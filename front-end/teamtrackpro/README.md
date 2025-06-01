# TeamTrackPro Frontend

This is the frontend application for TeamTrackPro, built with Next.js 14 and Material UI. It provides a modern, responsive user interface for managing projects, tickets, and team members.

## Features

### User Interface
- Modern, responsive design using Material UI
- Dark/Light theme support
- Role-based dashboard layouts
- Real-time updates
- Form validation
- Error handling
- Loading states

### Admin Dashboard
- Project statistics
- User management
- Project creation and assignment
- User role management

### Manager Dashboard
- Project overview
- Ticket management
- Team member assignment
- Progress tracking

### Developer Dashboard
- Assigned tickets
- Time tracking
- Status updates
- Comment system

## Technology Stack

- **Framework**: Next.js 14
- **UI Library**: Material UI
- **Language**: TypeScript
- **State Management**: React Hooks
- **API Client**: Axios
- **Form Handling**: React Hook Form
- **Validation**: Zod
- **Styling**: Tailwind CSS
- **Icons**: Material Icons

## Getting Started

### Prerequisites

- Node.js 18+
- npm or yarn
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/TeamTrackPro.git
cd TeamTrackPro/front-end/teamtrackpro
```

2. Install dependencies:
```bash
npm install
# or
yarn install
```

3. Create a `.env.local` file:
```
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

4. Start the development server:
```bash
npm run dev
# or
yarn dev
```

The application will be available at `http://localhost:3000`

## Project Structure

```
teamtrackpro/
├── app/                    # Next.js app directory
│   ├── admin/             # Admin routes
│   ├── manager/           # Manager routes
│   ├── developer/         # Developer routes
│   ├── login/             # Authentication pages
│   └── layout.tsx         # Root layout
├── components/            # Reusable components
│   ├── admin/            # Admin-specific components
│   ├── manager/          # Manager-specific components
│   ├── developer/        # Developer-specific components
│   ├── layouts/          # Layout components
│   └── ui/               # UI components
├── hooks/                # Custom React hooks
│   ├── use-auth.ts      # Authentication hook
│   ├── use-projects.ts  # Projects hook
│   ├── use-tickets.ts   # Tickets hook
│   └── use-users.ts     # Users hook
├── lib/                  # Utility functions
│   ├── api.ts           # API client
│   └── utils.ts         # Helper functions
├── types/               # TypeScript definitions
├── styles/              # Global styles
└── public/              # Static assets
```

## Development

### Key Components

1. **Authentication**
   - Login/Logout functionality
   - Password reset
   - JWT token management
   - Protected routes

2. **Project Management**
   - Project creation and editing
   - Project assignment
   - Project status tracking
   - Project timeline view

3. **Ticket System**
   - Ticket creation and management
   - Status updates
   - Time tracking
   - Comment system

4. **User Management**
   - User creation and editing
   - Role assignment
   - Profile management

### Available Scripts

- `npm run dev`: Start development server
- `npm run build`: Build for production
- `npm run start`: Start production server
- `npm run lint`: Run ESLint
- `npm run type-check`: Run TypeScript compiler

## State Management

The application uses React Hooks for state management:

- `useAuth`: Authentication state
- `useProjects`: Projects data
- `useTickets`: Tickets data
- `useUsers`: Users data

## API Integration

The frontend communicates with the backend API using Axios. The API client is configured in `lib/api.ts` with:

- Base URL configuration
- Authentication token management
- Request/Response interceptors
- Error handling

## Styling

The application uses a combination of:
- Material UI components
- Tailwind CSS for custom styling
- CSS Modules for component-specific styles

## Environment Variables

- `NEXT_PUBLIC_API_URL`: Backend API URL
- `NEXT_PUBLIC_APP_NAME`: Application name
- `NEXT_PUBLIC_APP_VERSION`: Application version

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License. 