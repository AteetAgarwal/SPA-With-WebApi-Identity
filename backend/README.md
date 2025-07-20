# SPA-With-WebApi-Identity

This project demonstrates a secure authentication and authorization flow for a backend Web API. The application uses Azure AD for authentication and implements role-based access control (RBAC) to manage user permissions.

## Features

- **OAuth 2.0 Authorization Code Flow**: The application uses the Authorization Code Flow for secure authentication.
- **Role-Based Access Control (RBAC)**: Users are assigned roles in Azure AD, and access to resources is restricted based on these roles.
- **Secure Token Management**: Tokens are managed entirely on the server, and HTTP-only cookies are used for session management.
- **Cross-Origin Support**: Configured to allow cross-origin requests from authorized clients.

## Prerequisites

1. **Azure AD Configuration**:
   - Register an application in Azure AD.
   - Define app roles (e.g., Admin, User) in the app registration.
   - Assign roles to users or groups in Azure AD.
2. **Development Environment**:
   - .NET 8 for the backend.
3. **Backend URL**:
   - Backend: `https://localhost:7035`

## Setup Instructions

### Backend Setup

1. Clone the repository and navigate to the `backend` folder:
   ```bash
   cd backend
   ```

2. Update the `appsettings.json` file with your Azure AD configuration:
   ```json
   {
     "AzureAd": {
       "Instance": "https://login.microsoftonline.com/",
       "TenantId": "your-tenant-id",
       "ClientId": "your-client-id",
       "ClientSecret": "your-client-secret",
       "CallbackPath": "/signin-oidc"
     }
   }
   ```

3. Run the backend:
   ```bash
   dotnet run
   ```

## Azure AD App Registration

To configure Azure AD for this application, follow these steps:

1. **Register the Application**:
   - Go to the Azure portal and navigate to **Azure Active Directory** > **App registrations**.
   - Click **New registration**.
   - Provide a name for the application (e.g., `SPA-With-WebApi-Identity`).
   - Set the **Supported account types** to "Accounts in this organizational directory only" (or as per your requirements).
   - Add the following redirect URI: `https://localhost:7035/signin-oidc`.
   - Click **Register**.

2. **Configure API Permissions**:
   - Go to the **API permissions** section of the app registration.
   - Click **Add a permission**.
   - Select **Microsoft Graph**.
   - Add the following delegated permissions:
     - `openid`
     - `profile`
     - `email`
   - Click **Grant admin consent** to grant these permissions for all users in the directory.

3. **Define App Roles**:
   - Go to the **App roles** section of the app registration.
   - Click **Create app role** and define roles such as `Admin` and `User`.
   - Example configuration for an app role:
     ```json
     {
       "allowedMemberTypes": ["User"],
       "description": "Administrator role",
       "displayName": "Admin",
       "id": "unique-guid-for-admin-role",
       "isEnabled": true,
       "value": "Admin"
     }
     ```
   - Assign these roles to users or groups in Azure AD.

4. **Configure Authentication**:
   - Go to the **Authentication** section of the app registration.
   - Add the following redirect URI: `https://localhost:7035/signin-oidc`.
   - Enable the following settings:
     - **ID tokens** (used for OpenID Connect authentication).

5. **Create a Client Secret**:
   - Go to the **Certificates & secrets** section of the app registration.
   - Click **New client secret**.
   - Provide a description and set an expiration period.
   - Copy the client secret value and store it securely. You will need this in the `appsettings.json` file.

6. **Update `appsettings.json`**:
   - Add the Azure AD configuration to your `appsettings.json` file:
     ```json
     {
       "AzureAd": {
         "Instance": "https://login.microsoftonline.com/",
         "TenantId": "your-tenant-id",
         "ClientId": "your-client-id",
         "ClientSecret": "your-client-secret",
         "CallbackPath": "/signin-oidc"
       }
     }
     ```

## Role-Based Access Control (RBAC)

1. **Define Roles in Azure AD**:
   - Navigate to your Azure AD app registration.
   - Define roles like `Admin` and `User` in the **App Roles** section.
   - Assign these roles to users or groups.

2. **Enforce Roles in the Backend**:
   - Use the `[Authorize(Roles = "RoleName")]` attribute to restrict access to specific endpoints.

   Example:
   ```csharp
   [ApiController]
   [Route("api/data")]
   public class DataController : ControllerBase
   {
       [HttpGet("admin")]
       [Authorize(Roles = "Admin")]
       public IActionResult GetAdminData()
       {
           return Ok(new { message = "This is admin data." });
       }

       [HttpGet("user")]
       [Authorize(Roles = "User")]
       public IActionResult GetUserData()
       {
           return Ok(new { message = "This is user data." });
       }

       [HttpGet("common")]
       [Authorize]
       public IActionResult GetCommonData()
       {
           return Ok(new { message = "This is common data for all authenticated users." });
       }
   }
   ```

3. **Debugging Claims**:
   - Add an endpoint to inspect the user's claims:
     ```csharp
     [HttpGet("claims")]
     [Authorize]
     public IActionResult GetClaims()
     {
         var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
         return Ok(claims);
     }
     ```

## Security Considerations

- **No Tokens in the Browser**: Tokens are managed entirely on the server, reducing the risk of token theft.
- **HTTP-Only Cookies**: Used for session management to prevent XSS attacks.
- **Role-Based Authorization**: Ensures that users can only access resources they are authorized for.

## Future Enhancements

- Add support for refresh tokens to extend session duration without requiring re-login.
- Implement additional logging and monitoring for security and performance.
- Add unit and integration tests for the backend.

