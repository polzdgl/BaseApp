# BaseApp Project

## Overview

BaseApp is a web application built using a Blazor Web Assembly and an ASP.NET Web API with Controllers. It uses Entity Framework Core with a repository pattern for data access and Microsoft SQL Server as the database. Logging is implemented using Serilog, and API documentation is provided through Swagger.

---

## Features

- **User Management**: Add new User (without Identity Registration), retrieve, and manage user data.
- **User Authentication**: Register and Login to Authenticate users using ASP.Net Identity Provider 
- **Data Validation**: Includes custom validation logic for phone numbers, email addresses, and dates of birth.
- **Logging**: Integrated with Serilog for structured and detailed logging.
- **API Documentation**: Interactive API documentation available via Swagger UI.
- **Automated Database Creation**: Automatically creates and migrates the database in development environments.
- **Unit Testing**: Unit tests implemented using xUnit and NSubstitute.

---

## Technologies Used

- **Frontend**: Blazor Web Assembly with Radzen Coponents
- **Backend**: ASP.NET Web API (with Controllers)
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core (with Repository Pattern)
- **Logging**: Serilog
- **API Documentation**: Swagger (OpenAPI)
- **Unit Testing**: xUnit with NSubstitute

---

## Setup Instructions

### Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server)
- Any IDE (Visual Studio, VS Code, JetBrains Rider)

### Steps to Run

1. Clone the repository:
   ```bash
   git clone hhttps://github.com/polzdgl/BaseApp.git
   ```
2. Open the solution in your IDE of choice.
3. Navigate to the `BaseApp.Server` project and update the connection string in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your-SQL-Server-Connection-String"
     }
   }
   ```

   Replace `Your-SQL-Server-Connection-String` with your actual connection string.

4. Set the `BaseApp.Server` project as the startup project.
5. Run the application in Development mode. The database and required tables will be created automatically.
6. Open the Swagger UI for API documentation:
   ```
   http://localhost:<your-port>/swagger
   ```

---

## Unit Testing

Unit tests are implemented using xUnit with mocking provided by NSubstitute. To run the tests:

1. Open the solution in your IDE.
2. Navigate to the `BaseApp.Tests` project.
3. Run the tests using the integrated test runner or via CLI:
   ```bash
   dotnet test
   ```

---

## Folder Structure

- **BaseApp.Server**: The ASP.NET Web API project with Controllers. 
- **BaseApp.Client**: The presentation layer with the Blazor Web Assembly UI with Radzen Components.
- **BaseApp.Shared**: Contains shared utilities, validation classes, and DTOs.
- **BaseApp.Tests**: Unit test project using xUnit and NSubstitute.
- **BaseApp.Data**: The data layer project responsible for managing all data-related operations.
- **BaseApp.ServiceProvider**: The business layer project with all the application logic.
---

## Additional Notes

- **Development Database**: The application is configured to automatically create and migrate the database in the development environment. No manual setup is required. If you wish to run the EF Core migration instead, use the Migration scripts in the next section.
- **Production Configuration**: Ensure to provide the correct connection string and configure the database appropriately for production deployments.
- **Logging**: All logs are structured and stored using Serilog for easy debugging and monitoring.

---

## EF Core Migration Scripts

- **Run this command to create new Migration**:
```bash
dotnet ef migrations add InitialData --project BaseApp.Data --startup-project BaseApp.Server`
```
- **Run this command to run EF Core Migration**:
 ```bash
dotnet ef database update --project BaseApp.Data --startup-project BaseApp.Server
```
---
