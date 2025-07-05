# LibraryWebApp

A web application for managing library resources, users, and book lending.

## Features

- User authentication and registration
- Book catalog management (add, edit, delete)
- Borrow and return books
- User profile management
- Admin dashboard

## Technologies Used

- ASP.NET Core / .NET
- Entity Framework Core
- SQL Server
- HTML, Bootstrap

## Getting Started

1. Clone the repository:
    ```bash git clone https://github.com/yourusername/LibraryWebApp.git```
2. Configure the database connection in `appsettings.json`.
3. Run database migrations:
    ```bash
    dotnet ef migrations add InitMigration
    ```
    ```bash
    dotnet ef database update
    ```
4. Start the application:
    ```bash
    dotnet run
    ```
