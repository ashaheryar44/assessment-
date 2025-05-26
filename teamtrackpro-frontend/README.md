# TeamTrackPro Frontend

This is the frontend application for TeamTrackPro, built with Next.js and Material UI.

## Prerequisites

- Node.js 18.x or later
- npm 9.x or later

## Setup

1. Install dependencies:
   ```bash
   npm install
   ```

2. Create a `.env.local` file in the root directory with the following content:
   ```
   NEXT_PUBLIC_API_URL=http://localhost:5001/api
   ```

## Development

To run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

## Building for Production

To create a production build:

```bash
npm run build
```

To start the production server:

```bash
npm start
```

## Features

- Modern UI with Material UI components
- Responsive design for all screen sizes
- Authentication with JWT
- Protected routes based on user roles
- Dashboard with project statistics
- Project management interface
- Team management interface

## Project Structure

```
src/
  ├── app/                 # Next.js app directory
  ├── components/          # Reusable components
  │   ├── auth/           # Authentication components
  │   └── layout/         # Layout components
  ├── lib/                # Utility functions and hooks
  │   ├── auth/          # Authentication service
  │   └── hooks/         # Custom React hooks
  └── types/             # TypeScript type definitions
``` 