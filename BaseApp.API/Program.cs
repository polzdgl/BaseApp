using BaseApp.API.AppStart;
using BaseApp.API.Middleware;
using BaseApp.API.Settings;
using BaseApp.Data.Context;
using BaseApp.Data.User.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
string connectionString = builder.Configuration.GetConnectionString("OneKlerenContext");
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

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

// Add Identity API Endpoints
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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

// Configure the HTTP request pipeline.
app.UseDefaultFiles();
app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Use the configured CORS policy
app.UseCors("ConfiguredCorsPolicy");

// Use the global exception handler middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Serilog Logging
app.UseSerilogRequestLogging();

// Add Identity
app.MapIdentityApi<User>();

// Only for development environment
if (app.Environment.IsDevelopment())
{
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

app.UseHttpsRedirection();

Log.Information("BaseApp is starting..");

app.Run();