## Demo
Download the following video from the `media` folder to view a demonstration of the application:

[Demo.mp4](media/Demo.mp4)

# Sensitive Words - Text Sanitization System

A comprehensive text sanitization and content moderation system built with Clean Architecture principles using .NET Core, Entity Framework, and MSSQL.

## Features

- **RESTful API** with full CRUD operations for sensitive word management
- **Real-time text sanitization** with customizable replacement characters
- **Swagger/OpenAPI documentation** for easy API exploration
- **ASP.NET MVC frontend** with admin panel and chat demo
- **Clean Architecture (DDD)** with proper separation of concerns

## Architecture

The project follows Clean Architecture principles with the following layers:

- **Domain Layer** (`SensitiveWordsClean.Domain`): Core business entities and domain interfaces
- **Application Layer** (`SensitiveWordsClean.Application`): Business logic, DTOs, and application services
- **Infrastructure Layer** (`SensitiveWordsClean.Infrastructure`): Data access and external services implementation
- **API Layer** (`SensitiveWordsClean.API`): RESTful Web API with Swagger documentation
- **Web Layer** (`SensitiveWordsClean.Web`): ASP.NET MVC frontend application

## Getting Started

## Technologies Used
- ASP.NET Core Web API
- ASP.NET MVC
- Domain-Driven Design (DDD)
- MSSQL
- Javascript and JQuery
- Swagger

## Getting Started
1. Clone the repository.
2. Build the solution using Visual Studio or `dotnet build`.
3. Update connection strings in `appsettings.json` if needed.
4. Run the API:
   - `dotnet run --project src/SensitiveWordsClean.API`
   - Default API port: `http://localhost:5291`
   - Swagger: `http://localhost:5291/swagger`
5. Run the Web frontend:
   - `dotnet run --project src/SensitiveWordsClean.Web`
   - Default Web port: `http://localhost:5258`

1. **Start the API** (runs on http://localhost:5291 or https://localhost:7203):
   ```bash
   cd src/SensitiveWordsClean.API
   dotnet run
   ```

2. **Start the Web application** (runs on http://localhost:5258 or https://localhost:7092):
   ```bash
   cd src/SensitiveWordsClean.Web
   dotnet run
   ```

#### Option 2: Use Visual Studio

1. Set multiple startup projects:
   - Right-click solution → Properties
   - Select "Multiple startup projects"
   - Set both `SensitiveWordsClean.API` and `SensitiveWordsClean.Web` to "Start"

## Production Deployment Walkthrough

To deploy this solution in a production environment, follow these recommended steps:

1. **Build and Publish Artifacts**
   - Use `dotnet publish` to generate release builds for both API and Web projects.   

2. **Database Setup**
   - Provision a production-grade MSSQL database (cloud or on-premises).
   - Apply migrations using Entity Framework Core or your preferred tool.
   - Secure connection strings and credentials.

3. **Configuration**
   - Set environment variables for production (e.g., `ASPNETCORE_ENVIRONMENT=Production`).
   - Update `appsettings.Production.json` with production values (connection strings, logging, etc.).

4. **Web Server**
   - Host the API and Web apps behind a reverse proxy (e.g., IIS, Nginx, Apache, Azure App Service).
   - Configure HTTPS and SSL certificates for secure communication.
   - Set up proper routing for API and Web endpoints.

5. **Scaling & Reliability**
   - Use containers (Docker) or cloud services for scalability and easier deployment.
   - Consider using Kubernetes for high availability.
   - Enable health checks and monitoring (e.g., Application Insights).

6. **Security**
   - Implement authentication and authorization (JWT, OAuth, etc.).
   - Restrict access to sensitive endpoints and data.
   - Regularly update dependencies and patch vulnerabilities.

7. **CI/CD Pipeline**
   - Automate build, test, and deployment using CI/CD tools (GitHub Actions, Azure DevOps, etc.).
   - Run automated tests before deployment.

8. **Backup & Disaster Recovery**
   - Schedule regular database backups.
   - Document and test recovery procedures.

9. **Documentation & Support**
   - Provide clear documentation for operators and users.
   - Set up support channels and incident response plans.

This approach ensures a secure, scalable, and maintainable deployment for production environments.

## Performance Enhancements

To enhance the performance of this project, consider implementing:

1. **Database Optimization**:
   - Database indexing on commonly queried fields
   - Query optimization and pagination

23. **API Performance**:
   - Rate limiting to prevent abuse
   - Response compression (gzip)
   - API versioning for backward compatibility

3. **Text Processing**:
   - Advanced text search algorithm

## Additional Enhancements

To make this project more complete, consider adding:

1. **Security Features**:
   - Authentication and authorization (JWT tokens)
   - API key management
   - Input validation and sanitization

2. **Monitoring & Logging**:
   - Structured logging with Serilog
   - Application Performance Monitoring (APM)
   - Health checks for API endpoints

3. **DevOps & Deployment**:
   - Docker containerization
   - CI/CD pipelines
   - Configuration management
   - Database migrations

4. **User Interface**:
   - Real-time dashboard for monitoring
   - Bulk import/export functionality
   - Advanced filtering and search
   - User management and roles

## Technology Stack

- **.NET 9.0**: Framework
- **ASP.NET Core**: Web API and MVC
- **MSSQL**: Database
- **Swagger/OpenAPI**: API documentation
- **Bootstrap**: Frontend UI framework
- **Font Awesome**: Icons

## Project Structure

```
SensitiveWordsClean/
├── src/
│   ├── SensitiveWordsClean.Domain/         # Domain entities and interfaces
│   ├── SensitiveWordsClean.Application/    # Business logic and DTOs
│   ├── SensitiveWordsClean.Infrastructure/ # Data access and services
│   ├── SensitiveWordsClean.API/            # RESTful Web API
│   └── SensitiveWordsClean.Web/            # MVC Web application
├── tests/                                  # Unit and integration tests
└── SensitiveWordsClean.sln                 # Solution file
```

## License

This project is provided as-is for educational and demonstration purposes.
