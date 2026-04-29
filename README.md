# AutoDrive System

AutoDrive is a web-based driving school management system built with ASP.NET Core MVC.  
It supports user roles, course management, lessons, enrollments, and payments.

---

## Purpose

The system simulates a real-world driving school platform where:
- Users can register and manage profiles
- Students can enroll in driving courses
- Trainers manage lessons and schedules
- Admins manage the whole system

## User Roles

- Admin – full system control
- Trainer – manages lessons and students
- Student – enrolls in courses and attends lessons

---

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- PostgreSQL
- C#
- Razor Views
- Bootstrap
- JavaScript

---

## Features

### Authentication & Authorization
- User registration and login
- Role-based access (Admin / Trainer / Student)
- Secure session handling

### Course Management
- Create and manage driving courses
- Assign trainers and students
- Track course progress

### Lessons System
- Lesson scheduling
- Lesson status tracking
- Lesson types and vehicles support

### Payments
- Payment tracking per course/enrollment

### Database
- PostgreSQL database
- Entity Framework Core Code First
- Migrations support

---

## Architecture

- MVC (Model–View–Controller)
- Separation of concerns:
  - Controllers - business logic
  - Models - data structure
  - Views - UI layer
- EF Core for ORM
- Code First database approach

## Setup & Installation

### Install dependencies

Using Package Manager Console:
```powershell
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.4  
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -Version 8.0.4  
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.4
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.4
Install-Package Microsoft.AspNetCore.Identity.UI -Version 8.0.4
```

### Database Migration

Create initial migration:
```powershell
Add-Migration InitialCreate 
```

Apply migration:
```powershell
Update-Database
```

### Database connection

Initialize secrets:
```powershell
dotnet user-secrets init
```

Set connection string:
```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..."
```

## Screenshots

### Home Page
![Home Page](wwwroot/images/screenshots/Home.png)

### Registration Page
![Registration Page](wwwroot/images/screenshots/Registration.png)

### Login Page
![Login Page](wwwroot/images/screenshots/Login.png)
