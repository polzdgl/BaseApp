EF Core Migration Command:

To Create Migration (Run it from base directory):
dotnet ef migrations add InitialData --project BaseApp.Data --startup-project BaseApp.API

To Apply Migration
dotnet ef database update --project BaseApp.Data --startup-project BaseApp.API

