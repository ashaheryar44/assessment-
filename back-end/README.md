# TeamTrackPro Backend API

This is the backend API for TeamTrackPro, a project management system built with .NET 8. The API provides endpoints for user authentication, project management, ticket tracking, and user management.

## Architecture

### Database Schema
```
User
└───< can belong to many >─── Project
Project
└───< has many >─── Ticket
Ticket
└─── assigned to one ───> User
ActivityLog
└─── logs action by ───> User
```

### Key Components

1. **Authentication & Authorization**
   - JWT-based authentication
   - Role-based access control (Admin, Manager, Developer)
   - Password hashing with BCrypt
   - Token refresh mechanism

2. **Project Management**
   - CRUD operations for projects
   - Project assignment to managers
   - Project status tracking
   - Project timeline management

3. **Ticket System**
   - Ticket creation and management
   - Status tracking (Todo, In Progress, QA, Done)
   - Time tracking
   - Comment system
   - Priority levels

4. **User Management**
   - User registration and profile management
   - Role assignment
   - Password reset functionality
   - User activity logging

## Technology Stack

- **Framework**: .NET 8
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Authentication
- **API Documentation**: Swagger/OpenAPI
- **Logging**: Serilog
- **Validation**: FluentValidation
- **Testing**: xUnit

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/TeamTrackPro.git
cd TeamTrackPro/back-end
```

2. Update the database connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TeamTrackPro;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Install dependencies:
```bash
dotnet restore
```

4. Run database migrations:
```bash
dotnet ef database update
```

5. Start the development server:
```bash
dotnet run
```

The API will be available at `http://localhost:5000`

### API Documentation

Once the server is running, you can access the Swagger documentation at:
```
http://localhost:5000/swagger
```

## Development

### Project Structure

```
TeamTrackPro.API/
├── Controllers/         # API endpoints
├── Models/             # Database entities
├── DTOs/               # Data transfer objects
├── Services/           # Business logic
├── Data/               # Database context
├── Migrations/         # Database migrations
├── Middleware/         # Custom middleware
├── Helpers/            # Utility classes
└── Validators/         # Input validation
```

### Key Files

- `Program.cs`: Application entry point and configuration
- `appsettings.json`: Application configuration
- `TeamTrackPro.API.csproj`: Project dependencies
- `Data/ApplicationDbContext.cs`: Database context
- `Controllers/*.cs`: API endpoints
- `Services/*.cs`: Business logic implementation

### Running Tests

```bash
dotnet test
```

## Deployment

1. Build the application:
```bash
dotnet build -c Release
```

2. Publish the application:
```bash
dotnet publish -c Release -o ./publish
```

3. Deploy the contents of the `publish` directory to your hosting environment.

## Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to "Development", "Staging", or "Production"
- `ConnectionStrings__DefaultConnection`: Database connection string
- `Jwt__Key`: Secret key for JWT token generation
- `Jwt__Issuer`: JWT token issuer
- `Jwt__Audience`: JWT token audience

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.
