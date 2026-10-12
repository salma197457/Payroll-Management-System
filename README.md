# Payroll Management System

A full-stack Payroll Management System developed using ASP.NET Core Web API, Angular, Entity Framework Core, and SQL Server.

## Technologies

**Backend**
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server
- Hangfire
- Clean Architecture

**Frontend**
- Angular 22
- TypeScript
- HTML and CSS

## Features

- Department Management
- Job Grade Management
- Employee Management
- Department Incentives
- Experience Incentives
- Absence Deduction Rules
- Employee Absence Management
- Monthly Payroll Generation
- Employee Report
- Attendance Report
- Incentives & Deductions Report
- Salary Report
- Report Printing

## Getting Started

### Backend

1. Install the .NET 10 SDK and SQL Server.
2. Configure the database connection string.
3. Restore the NuGet packages.
4. Apply Entity Framework Core migrations.
5. Run the Payroll API.

### Frontend

1. Install Node.js and npm.
2. Navigate to the `payroll-ui` folder.
3. Run `npm install`.
4. Start the Angular development server using `npm start`.
5. Open `http://localhost:4200`.

The Angular development proxy is configured to communicate with the local ASP.NET Core API.

## API Documentation

Swagger is available at `/swagger` when enabled in the API environment.

## Background Jobs

Hangfire is used for scheduled monthly payroll generation.

## Reports

The application provides employee, attendance, incentives and deductions, and salary reports with filtering and printing support.
