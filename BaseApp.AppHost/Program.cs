var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BaseApp_ApiService>("apiservice");

builder.AddProject<Projects.BaseApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
