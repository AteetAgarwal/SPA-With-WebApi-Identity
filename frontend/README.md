# SPA-With-WebApi-Identity

This is a sample application demonstrating a secure authentication and authorization flow for a Single Page Application (SPA) interacting with a backend Web API. The application avoids storing access tokens in the browser and instead relies on cookies for session management.

## Authentication Flow

This application uses the **OAuth 2.0 Authorization Code Flow with PKCE** for authentication and authorization. The flow is implemented as follows:

1. The SPA redirects the user to the backend for login.
2. The backend handles the OAuth 2.0 Authorization Code Flow with PKCE:
   - Exchanges the authorization code for tokens (access token, refresh token, and ID token).
   - Stores the tokens securely on the server.
3. The backend sets an HTTP-only cookie to maintain the user's session.
4. The SPA communicates with the backend to:
   - Check the user's authentication status.
   - Fetch user details (e.g., name, email) from the backend.
5. The backend handles all token management and ensures secure communication with the identity provider.

## Features

- **Secure Authentication**: The SPA does not store tokens in the browser, reducing the risk of token theft.
- **Session Management**: The backend uses HTTP-only cookies to maintain the user's session.
- **User Info Retrieval**: The SPA can fetch user details from the backend without directly handling tokens.
- **Logout Support**: The user can log out, and the backend clears the session and cookies.

## Prerequisites

1. **Identity Provider**: An identity provider (e.g., Azure AD, Auth0) configured to support OAuth 2.0 Authorization Code Flow with PKCE.
2. **Development Environment**:
   - .NET 8 for the backend.
   - A modern browser for the SPA.
3. **Frontend and Backend URLs**:
   - Frontend: `http://localhost:3000`
   - Backend: `https://localhost:7035`

## Setup Instructions

### Backend Setup

1. Clone the repository and navigate to the `backend` folder:
   ```bash
   cd backend
2. dotnet run

### Frontend Setup

1. cd frontend
2. Serve the frontend using a static file server (e.g., serve):
   npx serve -l 3000
3. Ensure the serve.json file is configured to allow cross-origin requests:
     {
      "headers": [
        {
          "source": "**",
          "headers": [
            {
              "key": "Access-Control-Allow-Origin",
              "value": "https://localhost:7035"
            },
            {
              "key": "Access-Control-Allow-Credentials",
              "value": "true"
            }
          ]
        }
      ]
    }
4. Open the frontend in your browser:
    http://localhost:3000

## Identity Provider Configuration

1. Register an application in your identity provider (e.g., Azure AD).
2. Configure the following redirect URIs:
  https://localhost:7035/signin-oidc (backend)
  http://localhost:3000 (frontend)
3. Enable the following scopes:
  openid
  profile
  email

## How It Works

1. Login:
  - The SPA calls the backend's /api/auth/login endpoint.
  - The backend redirects the user to the identity provider for authentication.
  - After successful login, the backend sets an HTTP-only cookie and redirects the user back to the SPA.
2. Check Authentication Status:
  - The SPA calls the backend's /api/auth/is-authenticated endpoint to check if the user is logged in.
  - The backend verifies the session and returns the user's authentication status and basic details.
3. Fetch User Profile:
  - The SPA calls the backend's /api/auth/profile endpoint to fetch detailed user information.
  - The backend retrieves the user's profile using the access token stored on the server.
4. Logout:
  - The SPA calls the backend's /api/auth/logout endpoint.
  - The backend clears the session and cookies and redirects the user back to the SPA.


## Security Considerations

1. No Tokens in Browser: Tokens are managed entirely by the backend, reducing the risk of token theft.
2. HTTP-Only Cookies: The backend uses secure, HTTP-only cookies for session management.
3. PKCE: The OAuth 2.0 Authorization Code Flow with PKCE ensures secure token exchange.

## Folder Structure
.gitignore
[README.md](http://_vscodecontentref_/1)
backend/
    appsettings.Development.json
    appsettings.json
    Controllers/
        AuthController.cs
    Program.cs
frontend/
    [app.js](http://_vscodecontentref_/2)
    [index.html](http://_vscodecontentref_/3)
    [serve.json](http://_vscodecontentref_/4)
    [style.css](http://_vscodecontentref_/5)

## Future Enhancements

1. Add support for refresh tokens to extend session duration without requiring re-login.
2. Implement role-based access control (RBAC) for fine-grained authorization.
3. Add unit and integration tests for both the frontend and backend.