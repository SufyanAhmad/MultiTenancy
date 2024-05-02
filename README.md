# Multi-Tenancy ASP.NET Core Project
### Overview
This project is a multi-tenancy web application built using ASP.NET Core. It provides a scalable and efficient solution for hosting multiple </br>tenants (clients or organizations) within a single application instance, each with its own isolated data and configurations.
### Features
### Multi-Tenancy Support: Allows hosting multiple tenants within a single application instance.
Tenant Isolation: Ensures that each tenant's data and configurations are isolated from other tenants.
Flexible Configuration: Provides options for configuring tenant-specific settings such as database connections, permissions, etc.
### Authentication & Authorization: 
Implements authentication and authorization mechanisms to control access based on tenant roles and permissions.
### Scalability: 
Designed to be scalable, allowing easy addition of new tenants without affecting existing ones.
## Technologies Used
### ASP.NET Core: 
Backend framework for building web APIs.
### Entity Framework Core (EF Core):
 ORM for database operations and multi-tenancy data isolation.
### Identity Framework: 
Provides user authentication and authorization features.
### SQL Server: Database for storing tenant data and application settings.

### Dependency Injection: 
Utilized for managing services and components within the application.
## Getting Started
Follow these steps to set up the project locally:
1. Clone project
2. Run database scripts (development and production)
3. Run Application and set the tenant name development  for DevelopmentDB and production for ProductionDB
Usage
4. Manage tenant-specific settings and configurations.
5. Authenticate users based on tenant context.
6. Implement tenant-specific features and customization based on requirements.
