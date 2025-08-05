# SPA With WebApi Identity

_Last Updated: 2025-08-05_

## Overview

This repository demonstrates a secure authentication architecture using:
- **Backend**: ASP.NET Core Web API for authentication and token management
- **BFF (Backend For Frontend)**: ASP.NET Core acting as a proxy and session manager for the SPA
- **Frontend**: Single Page Application (SPA) using JavaScript, HTML, and CSS

The solution is designed for modern authentication scenarios, supporting secure token storage and separation of concerns.

## Directory Structure

```
.
├── backend/           # ASP.NET Core Web API for authentication
│   ├── Controllers/
│   ├── Program.cs
│   ├── TokenStore.cs
│   └── ...
├── bff-with-spa/      # ASP.NET Core BFF (Backend For Frontend)
│   ├── Controllers/
│   ├── Program.cs
│   ├── TokenStore.cs
│   └── wwwroot/spa/   # Static files for SPA
│       └── index.html
├── frontend/          # Standalone SPA (if served separately)
│   ├── app.js
│   ├── index.html
│   ├── style.css
│   └── serve.json
└── README.md
```

## Getting Started

### Prerequisites

- [.NET 6+ SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (if developing the SPA separately)
- Modern web browser

### Backend Setup

1. Navigate to the backend directory:
   ```sh
   cd backend
   ```
2. Restore dependencies and run:
   ```sh
   dotnet restore
   dotnet run
   ```

### BFF Setup

1. Navigate to the BFF directory:
   ```sh
   cd bff-with-spa
   ```
2. Restore dependencies and run:
   ```sh
   dotnet restore
   dotnet run
   ```

### Frontend Setup (if running separately)

1. Navigate to the frontend directory:
   ```sh
   cd frontend
   ```
2. Serve the SPA (using a static server or the provided config):
   ```sh
   # Example using http-server (install globally if needed)
   npx http-server .
   ```

## Usage

- Access the SPA via the BFF endpoint (typically `http://localhost:5000` or as configured).
- The BFF handles authentication, session management, and proxies API requests to the backend.

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements.

## License

Specify your license here (e.g., MIT, Apache 2.0).

## Credits

Thanks to all contributors and the open-source community.
