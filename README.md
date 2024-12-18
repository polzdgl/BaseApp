Sure! I've fixed the grammar and formatted it for a Git repository README file:

---

# BaseApp Project

## Overview
BaseApp is a web application built using a Blazor Web frontend and an ASP.NET Web API backend. It uses Entity Framework Core with a repository pattern for data access and Microsoft SQL Server as the database. Logging is implemented using Serilog, and API documentation is provided through Swagger.

---

## Features
- **User Management**: Add, retrieve, and manage user data.
- **Data Validation**: Includes custom validation logic for phone numbers, email addresses, and dates of birth.
- **Logging**: Integrated with Serilog for structured and detailed logging.
- **API Documentation**: Interactive API documentation available via Swagger UI.
- **Automated Database Creation**: Automatically creates and migrates the database in development environments.
- **Unit Testing**: Unit tests implemented using xUnit and NSubstitute.

---

## Technologies Used
- **Frontend**: Blazor Web
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
   git clone https://github.com/your-repo/BaseApp.git
   ```
2. Open the solution in your IDE of choice.
3. Navigate to the `BaseApp.API` project and update the connection string in `appsettings.json`:
   ```json
   {
       "ConnectionStrings": {
           "DefaultConnection": "Your-SQL-Server-Connection-String"
       }
   }
   ```
   Replace `Your-SQL-Server-Connection-String` with your actual connection string.

4. Set the `AppHost` project as the startup project.
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

- **BaseApp.API**: The ASP.NET Web API project.
- **BaseApp.AppHost**: The main project responsible for hosting and running the application.
- **BaseApp.Shared**: Contains shared utilities, validation classes, and DTOs.
- **BaseApp.Tests**: Unit test project using xUnit and NSubstitute.
- **BaseApp.Data**: The data layer project responsible for managing all data-related operations.
- **BaseApp.ServiceProvider**: The business layer project with all the application logic.
- **BaseApp.Web**: The presentation layer with the UI.

---

## Additional Notes
- **Development Database**: The application is configured to automatically create and migrate the database in the development environment. No manual setup is required.
- **Production Configuration**: Ensure to provide the correct connection string and configure the database appropriately for production deployments.
- **Logging**: All logs are structured and stored using Serilog for easy debugging and monitoring.

---
