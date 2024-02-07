# aspRESTwebAPI

This repository contains a straightforward ASP.NET Web API project designed to facilitate CRUD (Create, Read, Update, Delete) operations for each entity (Customer, Product, Order). 
It utilizes essential features such as Repository Control, Dependency Injection, and the Entity Framework as the Object-Relational Mapping (ORM) tool, alongside Fluent API and AutoMapper for efficient data manipulation.
Additionally, the repository includes an AppDbInitializer class created to seed initial data into the database.

## Prerequisites

This project was developed using Visual Studio 2022 and .NET 6.0. 
Below is the list of NuGet packages utilized:

- AutoMapper(12.0.1)
- AutoMapper.Extension.Microsoft.DependencyInjection(12.0.1)
- EntityFramework(6.4.4)
- Microsoft.EntityFrameworkCore (6.0.26)
- Microsoft.EntityFrameworkCore.Design (6.0.26)
- Microsoft.EntityFrameworkCore.SqlServer (6.0.26)
- Microsoft.EntityFrameworkCore.Tools (6.0.26)
- Swashbuckle.AspNetCore (6.5.0)

## Installation

After cloning this repository, follow these steps to run the project:

- Update the DefaultConnectionString in appsettings.json to match your database connection details.
- Open the Package Manager Console and create a new migration by executing the command Add-Migration InitialMigration, then apply the migration by executing Update-Database.
- Upon running the project, seed data will be automatically populated into your database.

## API Documentation

The API documentation is implemented using Swagger UI. 
After running the project, you can access the Swagger UI interface to explore and interact with the API endpoints. 
Simply navigate to the /swagger endpoint in your web browser to view the documentation and test the available endpoints.
