using BaseApp.Data.Context;
using BaseApp.Data.User.Models;
using BaseApp.Server.AppStart;
using BaseApp.Server.Components;
using BaseApp.Server.Middleware;
using BaseApp.Server.Settings;
using BaseApp.ServiceProvider.Company.Porvider;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Radzen;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


// Serilog Configuration
string connectionString = builder.Configuration.GetConnectionString("BaseAppContext");
string loggingFileLocation = builder.Configuration.GetSection("LogginFileLocation").Value;
// Load CORS settings from appsettings.json
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

SerilogSettings serilogSettings = new SerilogSettings();
var loggerConfiguration = new LoggerConfiguration()
       .ReadFrom.Configuration(builder.Configuration)
       .Enrich.FromLogContext()
       .Enrich.WithMachineName()
       .Enrich.WithProcessName()
       .Enrich.WithProcessId()
       .Enrich.WithThreadId()
       .Enrich.WithCorrelationId()
       .Enrich.WithClientIp()
       .Enrich.WithRequestHeader("Request-Header")
       .WriteTo.MSSqlServer(connectionString, serilogSettings.MSSqlSinkOptions, null, null,
        global::Serilog.Events.LogEventLevel.Information, columnOptions: serilogSettings.GetColumnOptions());

if (!string.IsNullOrEmpty(loggingFileLocation))
{
    loggerConfiguration.WriteTo.File(path: loggingFileLocation,
                     outputTemplate: "{Timestamp:o} [{Level:u3}] ({Application}/{MachineName}/{CorrelationId}/{ProcessName}/{ProcessId}/{ClientIp}/{RequestHeader}) {Message} {NewLine}{Exception}",
                     rollingInterval: RollingInterval.Day,
                     retainedFileCountLimit: 7);
}

Log.Logger = loggerConfiguration.CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddLogging(logBuilder => logBuilder.AddSerilog());

// Inject DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
    options =>
    {
        options.CommandTimeout(300); // 5 mins in seconds
    }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution), ServiceLifetime.Scoped);

// Set default Culture for all new threads
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

// Add Health Check
builder.Services.AddHealthChecks()
    .AddSqlServer(
            connectionString: connectionString,
            healthQuery: "SELECT 1;",
            name: "sql",
            failureStatus: HealthStatus.Degraded,
            tags: ["db", "sql", "sqlserver"]);


// Add services to the container.
builder.Services.AddRazorComponents()
      .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();

builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "BaseAppTheme";
    options.Duration = TimeSpan.FromDays(365);
});

// Add HttpClient with StandardResilency
builder.Services.AddHttpClient<SecurityExchangeProvider>().AddStandardResilienceHandler();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();


builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
{
    // Configure Identity options 
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 12;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// For RequestHeader
builder.Services.AddHttpContextAccessor();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerSetting>();
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Inject App Services
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(BaseApp.Client._Imports).Assembly);

// Health Check middleware
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

}).RequireHost();

// Use the configured CORS policy
app.UseCors("ConfiguredCorsPolicy");

// Use the global exception handler middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Serilog Logging
app.UseSerilogRequestLogging();

// Add Identity
app.MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    // Apply Database changes
    await using (var serviceScope = app.Services.CreateAsyncScope())
    await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        await dbContext.Database.EnsureCreatedAsync();
    }

    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(docExpansion: Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
    app.MapOpenApi();

    // Log any issue with starting Serilog
    Serilog.Debugging.SelfLog.Enable(msg =>
    {
        Debug.Print(msg);
        Debugger.Break();
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

Log.Information("BaseApp is starting..");

app.Run();