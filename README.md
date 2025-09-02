# Sensitive Words Clean - Text Sanitization System

A comprehensive text sanitization and content moderation system built with Clean Architecture principles using .NET Core, Entity Framework, and MSSQL.

## Features

- **RESTful API** with full CRUD operations for sensitive word management
- **Real-time text sanitization** with customizable replacement characters
- **Category-based word management** for better organization
- **Active/inactive word status** for flexible content moderation
- **Swagger/OpenAPI documentation** for easy API exploration
- **ASP.NET MVC frontend** with admin panel and chat demo
- **Clean Architecture (DDD)** with proper separation of concerns
- **Entity Framework Core** with MSSQL database support

## Architecture

The project follows Clean Architecture principles with the following layers:

- **Domain Layer** (`SensitiveWordsClean.Domain`): Core business entities and domain interfaces
- **Application Layer** (`SensitiveWordsClean.Application`): Business logic, DTOs, and application services
- **Infrastructure Layer** (`SensitiveWordsClean.Infrastructure`): Data access and external services implementation
- **API Layer** (`SensitiveWordsClean.API`): RESTful Web API with Swagger documentation
- **Web Layer** (`SensitiveWordsClean.Web`): ASP.NET MVC frontend application

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- MSSQL Server or LocalDB
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository or extract the project files
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

### Database Setup

The application uses MSSQL LocalDB by default. The database will be automatically created when the API starts for the first time.

**Connection String** (in `appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SensitiveWordsCleanDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Running the Application

#### Option 1: Run API and Web separately

1. **Start the API** (runs on https://localhost:5001):
   ```bash
   cd src/SensitiveWordsClean.API
   dotnet run
   ```

2. **Start the Web application** (runs on https://localhost:5002):
   ```bash
   cd src/SensitiveWordsClean.Web
   dotnet run
   ```

#### Option 2: Use Visual Studio

1. Set multiple startup projects:
   - Right-click solution → Properties
   - Select "Multiple startup projects"
   - Set both `SensitiveWordsClean.API` and `SensitiveWordsClean.Web` to "Start"

## Usage

### API Documentation

Once the API is running, access the Swagger documentation at:
- **Swagger UI**: https://localhost:5001 (root URL)
- **OpenAPI spec**: https://localhost:5001/swagger/v1/swagger.json

### Web Interface

Access the web application at https://localhost:5002 (when running Web project):

- **Home Page**: Overview of system features
- **Admin Panel**: Manage sensitive words (CRUD operations)
- **Chat Demo**: Test text sanitization in real-time

### API Endpoints

#### Sensitive Words Management
- `GET /api/sensitivewords` - Get all sensitive words
- `GET /api/sensitivewords/{id}` - Get specific sensitive word
- `GET /api/sensitivewords/active` - Get active sensitive words
- `GET /api/sensitivewords/category/{category}` - Get words by category
- `POST /api/sensitivewords` - Create new sensitive word
- `PUT /api/sensitivewords/{id}` - Update existing sensitive word
- `DELETE /api/sensitivewords/{id}` - Delete sensitive word

#### Text Sanitization
- `POST /api/textsanitization/sanitize` - Sanitize text by replacing sensitive words

### Sample API Requests

#### Create a Sensitive Word
```json
POST /api/sensitivewords
{
  "word": "badword",
  "category": "Profanity",
  "createdBy": "Admin"
}
```

#### Sanitize Text
```json
POST /api/textsanitization/sanitize
{
  "text": "This contains a badword in the message",
  "replacementCharacter": "*"
}
```

**Response:**
```json
{
  "originalText": "This contains a badword in the message",
  "sanitizedText": "This contains a ******* in the message",
  "detectedWords": ["badword"]
}
```

## Performance Enhancements

To enhance the performance of this project, consider implementing:

1. **Caching**: 
   - Redis for caching active sensitive words
   - Memory caching for frequently accessed data

2. **Database Optimization**:
   - Database indexing on commonly queried fields
   - Query optimization and pagination

3. **API Performance**:
   - Rate limiting to prevent abuse
   - Response compression (gzip)
   - API versioning for backward compatibility

4. **Text Processing**:
   - Batch processing for multiple text sanitization requests
   - Asynchronous processing for large texts
   - Compiled regex patterns for better performance

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

3. **Advanced Features**:
   - Machine learning for automatic sensitive word detection
   - Severity levels for different types of words
   - Whitelist functionality for exceptions
   - Multi-language support

4. **DevOps & Deployment**:
   - Docker containerization
   - CI/CD pipelines
   - Configuration management
   - Database migrations

5. **User Interface**:
   - Real-time dashboard for monitoring
   - Bulk import/export functionality
   - Advanced filtering and search
   - User management and roles

## Technology Stack

- **.NET 9.0**: Framework
- **ASP.NET Core**: Web API and MVC
- **Entity Framework Core**: ORM
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

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is provided as-is for educational and demonstration purposes.
