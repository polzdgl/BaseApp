var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BaseApp_API>("apiservice");

builder.AddProject<Projects.BaseApp_WebUI>("webfrontend")
    .WithExternalHttpEndpoints(options =>
    {
        options.HttpEndpointOptions.ListenAddress = "http://localhost:5002"; // Use a fixed port
    })
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
