# TeamTrackPro - Project Management System

TeamTrackPro is a comprehensive project management system built with Next.js (frontend) and .NET (backend). It provides role-based access control for managing projects, tickets, and team members.

## Features

### Admin Features
- Dashboard with project and user statistics
- Create and manage projects
- Assign projects to managers
- Create and manage users (managers and developers)
- View project and user lists

### Manager Features
- View assigned projects
- Create and manage tickets
- View project details
- Track ticket progress

### Developer Features
- View projects with assigned tickets
- Update ticket status
- Add time spent on tickets
- Add comments to tickets

## Tech Stack

### Frontend
- Next.js 14
- Material UI
- TypeScript
- React Hooks
- Axios for API calls

### Backend
- .NET 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- RESTful API

## Prerequisites

- Node.js 18+ and npm
- .NET 8 SDK
- SQL Server
- Git

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
```bash
cd back-end/TeamTrackPro.API
```

2. Update the database connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TeamTrackPro;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Run database migrations:
```bash
dotnet ef database update
```

4. Start the backend server:
```bash
dotnet run
```

The backend API will be available at `http://localhost:5000`

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd front-end/teamtrackpro
```

2. Install dependencies:
```bash
npm install
```

3. Create a `.env.local` file in the frontend directory:
```
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

4. Start the development server:
```bash
npm run dev
```

The frontend will be available at `http://localhost:3000`

## Default Users

The system comes with the following default users:

### Admin
- Username: admin
- Password: admin123

### Manager
- Username: manager1
- Password: manager123

### Developer
- Username: dev1
- Password: dev123

## API Endpoints

### Authentication
- POST `/api/auth/login` - User login
- POST `/api/auth/change-password` - Change password
- POST `/api/auth/reset-password` - Reset password

### Projects
- GET `/api/projects` - Get all projects
- POST `/api/projects` - Create project
- GET `/api/projects/{id}` - Get project details
- PUT `/api/projects/{id}` - Update project
- DELETE `/api/projects/{id}` - Delete project

### Tickets
- GET `/api/tickets` - Get all tickets
- POST `/api/tickets` - Create ticket
- GET `/api/tickets/{id}` - Get ticket details
- PUT `/api/tickets/{id}` - Update ticket
- DELETE `/api/tickets/{id}` - Delete ticket
- POST `/api/tickets/{id}/comments` - Add comment

### Users
- GET `/api/users` - Get all users
- POST `/api/users` - Create user
- GET `/api/users/{id}` - Get user details
- PUT `/api/users/{id}` - Update user
- DELETE `/api/users/{id}` - Delete user

## Project Structure

### Frontend
```
front-end/teamtrackpro/
├── app/                    # Next.js app directory
├── components/            # Reusable components
├── hooks/                # Custom React hooks
├── lib/                  # Utility functions and API client
├── types/               # TypeScript type definitions
└── public/              # Static assets
```

### Backend
```
back-end/TeamTrackPro.API/
├── Controllers/         # API controllers
├── Models/             # Database models
├── DTOs/               # Data transfer objects
├── Services/           # Business logic
├── Data/               # Database context
└── Migrations/         # Database migrations
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License. 