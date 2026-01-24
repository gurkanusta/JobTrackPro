# JobTrackPro

JobTrackPro is a **backend-focused Job Application Tracking System** developed with **ASP.NET Core Web API** using **Clean Architecture** and **CQRS** principles.

The project is designed to demonstrate **professional backend architecture**, clear separation of concerns, and a scalable application structure, rather than focusing on complex frontend frameworks.

A lightweight **single-file HTML admin dashboard** is included and served directly from the backend for testing and demonstration purposes.

---

##  Features

- Create, update, and list job applications  
- Track application statuses:
  - Applied  
  - Interview  
  - Offer  
  - Rejected  
- Store interview dates, notes, job and company URLs  
- Clean Architecture with strict layer separation  
- CQRS pattern using Commands and Queries  
- MediatR for request handling  
- Entity Framework Core with SQL Server  
- Swagger / OpenAPI documentation  
- Built-in HTML admin panel (no React or frontend build required)

---

##  Architecture Overview

The project follows **Clean Architecture** principles and is structured as follows:

JobTrackPro
│
├── JobTrackPro.Api
│ ├── Controllers
│ ├── Swagger
│ ├── wwwroot (HTML Admin Panel)
│ └── Program.cs
│
├── JobTrackPro.Application
│ ├── Commands
│ ├── Queries
│ ├── DTOs
│ └── Interfaces
│
├── JobTrackPro.Domain
│ ├── Entities
│ └── Enums
│
└── JobTrackPro.Infrastructure
├── DbContext
├── EF Core Migrations
└── Persistence


### Key Design Decisions

- **CQRS** separates read and write operations  
- **MediatR** removes direct controller-to-business-logic dependencies  
- **Entity Framework Core** manages persistence and database migrations  
- **Dependency Injection** ensures loose coupling and maintainability  

---

##  Technologies Used

- ASP.NET Core Web API (.NET 8)  
- Entity Framework Core  
- SQL Server  
- MediatR  
- Swagger / OpenAPI  
- HTML, CSS, Vanilla JavaScript (Admin Panel)  

---

##  API Endpoints

| Method | Endpoint        | Description              |
|------|-----------------|--------------------------|
| GET  | /api/Jobs       | Get all job applications |
| GET  | /api/Jobs/{id}  | Get job by id            |
| POST | /api/Jobs       | Create a new job         |
| PUT  | /api/Jobs/{id}  | Update an existing job   |

All endpoints are fully documented via **Swagger UI**.

---

##  Built-in Admin Panel

Instead of using a heavy frontend framework, JobTrackPro includes a **single-file HTML admin dashboard**.

- Located at:  
JobTrackPro.Api/wwwroot/index.html
- Automatically served by ASP.NET Core  
- Opens directly when the project is run  
- Uses the same backend origin (no CORS issues)  

### Admin Panel Capabilities

- Dashboard with statistics and charts  
- Job list with filtering and pagination  
- Create and edit jobs via a drawer-based UI  
- Kanban board with drag & drop status updates  
- Interview calendar view  
- JSON import/export for local testing  
- Light and dark theme support  

---

## ▶ Running the Project

1. Configure the database connection in `appsettings.json`  
2. Apply migrations:
dotnet ef database update

3. Run the API:


dotnet run

4. Open in browser:


https://localhost:{PORT}/

Swagger UI is available at:
https://localhost:{PORT}/swagger


---

## 🔄 Differences from Previous JobTrack Project

| Previous JobTrack            | JobTrackPro                          |
|-----------------------------|--------------------------------------|
| Monolithic structure        | Clean Architecture                   |
| Controller-based logic      | CQRS with MediatR                    |
| EF Core used in controllers | EF Core isolated in Infrastructure   |
| Limited scalability         | Scalable and maintainable structure  |
| Basic CRUD focus            | Professional backend architecture   |
| No UI or test panel         | Built-in admin dashboard             |
| Tightly coupled layers      | Fully decoupled layers               |

JobTrackPro represents an **evolution from a simple CRUD-based project to a production-ready backend architecture**.

---

##  Project Purpose

This project was created to:
- Practice Clean Architecture and CQRS patterns  
- Build a real-world backend API  
- Demonstrate professional .NET backend development skills  
- Serve as a strong portfolio and CV project  

---

##  Notes

- The HTML admin panel is intentionally lightweight  
- The main focus is backend quality and architecture  
- The project can be easily extended with authentication, authorization, or a full frontend application  

---



GitHub: https://github.com/gurkanusta
