# EmployeeLeaveManagementSys

A simple ASP.NET Core web application for managing employees and leave requests.

## Overview

- Built with ASP.NET Core (.NET 10) using MVC and Razor views.
- Stores employees, departments and leave applications in a SQL Server database (configured via connection string).

## Requirements

- .NET 10 SDK
- SQL Server (or LocalDB)
- Visual Studio 2026 or VS Code

## Quick start

1. Clone or open the solution in Visual Studio:
   - Solution: `EmployeeLeaveManagementSys.slnx`
2. Update the connection string in `appsettings.json` (key: `DefaultConnection`) to point to your database.
3. Run database migrations or ensure the database schema exists for the `ApplicationDbContext`.
4. Start the app (F5 in Visual Studio). The app runs on HTTPS by default.

## Key features

- Create / edit / delete employees
- Departments management
- Leave application and approval workflow
- Paging, sorting and searching on the Employees list
- Centralized exception handling (UseExceptionHandler configured)

## Validation rules

- Employee phone number must contain exactly 10 digits (digits only). The app enforces this:
  - Server-side: [RegularExpression] attribute validates `^\d{10}$` on DTO/Model.
  - Client-side: `maxlength="10"` and `pattern="\d{10}"` added to input fields for create/edit.

If a search returns no results, the Employees list shows a friendly message: "No employees found for \"{search term}\".".

## Notes for developers

- Controllers validate `ModelState.IsValid` and will redisplay the form with validation messages if input is invalid.
- Exception handling uses `app.UseExceptionHandler("/Home/Error")` for safe error re-execution.

## Contributing

- Fixes and improvements are welcome. Please follow the existing code style and add tests where appropriate.

## License

 This project has no license file by default. Add one (e.g., MIT) if you plan to publish.

## Architecture

This project uses a simple layered MVC architecture optimized for clarity and small-team development.

- Presentation layer
  - Razor Views + MVC Controllers (Views under Views/Employee, Views/Home)
  - Responsible for rendering HTML, handling form input and client-side validation.

- Application layer
  - Controllers (EmployeeController, HomeController) and DTOs (EmployeeDto).
  - AutoMapper profiles (DTOs/*Profile.cs) convert between DTOs and EF models.

- Domain / Data layer
  - EF Core DbContext: EmployeeLeaveManagementSys.Data.ApplicationDbContext manages entities under Models/ (Employee, Department, LeaveRequest).
  - Persistence uses SQL Server configured with the `DefaultConnection` in appsettings.json.

- Infrastructure
  - Middleware: exception handling is configured with app.UseExceptionHandler("/Home/Error").
  - Logging: built-in ASP.NET Core logging via ILogger<T>.

Data flow (simple):

Browser -> Controller (Model binding, validation) -> AutoMapper -> DbContext -> SQL Server

If an exception occurs: middleware (UseExceptionHandler) re-executes to /Home/Error where the error view is shown and the exception is logged.

Key files and locations

- Program.cs          - app configuration and middleware pipeline
- Controllers/*       - controller logic and request handling
- Views/*             - Razor pages / UI
- DTOs/*              - data transfer objects and AutoMapper profiles
- Models/*            - EF Core entity models
- Data/ApplicationDbContext.cs - EF Core context and DbSets

Diagram (ASCII):

  [Browser]
	  |
	  v
  [Controller] -> [DTO / AutoMapper] -> [DbContext] -> [SQL Server]
	  |
	  v
  [Views / Validation / Client scripts]

Non-functional concerns

- Validation: server-side RegularExpression attributes and client-side pattern/maxlength.
- Error handling: centralized via UseExceptionHandler, errors are logged using ILogger.
- Security: ensure connection string and secrets are stored securely; consider HTTPS and authentication for production.

## Project structure

Top-level layout (important files/folders):

- /EmployeeLeaveManagementSys.slnx       - Solution file
- /EmployeeLeaveManagementSys/          - Web project root
  - Program.cs                          - Application startup and middleware
  - appsettings.json                    - Configuration (connection strings, etc.)
  - /Controllers/                       - MVC controllers (EmployeeController, HomeController)
  - /Views/                             - Razor views (Views/Employee, Views/Home, Shared)
  - /Models/                            - EF Core entity models (Employee, Department, LeaveRequest)
  - /DTOs/                              - DTOs and AutoMapper profiles
  - /Data/                              - ApplicationDbContext and data access
  - /Middleware/                        - Custom middleware (if present)
  - /wwwroot/                           - Static files (css, js)
  - /Migrations/                        - EF Core migrations (if used)

This should help you quickly find where to update validation, views, controllers, or database code.
