# SPA-With-WebApi-Identity

This project demonstrates a secure authentication and authorization flow for a backend Web API using ASP.NET Core, with a minimal Single Page Application (SPA) frontend. The application uses Azure AD for authentication and implements role-based access control (RBAC) to manage user permissions.

---

## Project Structure

```
/
├── Program.cs
├── TokenStore.cs
├── Controllers/
│   └── AuthController.cs
├── appsettings.json
├── appsettings.Development.json
├── wwwroot/
│   └── spa/
│       └── index.html
├── bff-with-spa.csproj
├── bff-with-spa.sln
└── ...
```

- **Backend:** ASP.NET Core Web API (project root)
- **Frontend:** Static HTML/JavaScript SPA in [`wwwroot/spa/index.html`](wwwroot/spa/index.html:1)

---

## Getting Started

### Prerequisites

- **Azure AD Configuration**
  - Register an application in Azure AD.
  - Define app roles (e.g., Admin, User) in the app registration.
  - Assign roles to users or groups in Azure AD.
- **Development Environment**
  - [.NET 8 SDK](https://dotnet.microsoft.com/download) for the backend.

### Setup Instructions

#### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd <repo-root>
```

#### 2. Configure Azure AD

Update the `appsettings.json` file in the project root with your Azure AD configuration:

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

#### 3. Run the Backend

From the project root:

```bash
dotnet run
```

The backend will be available at: `https://localhost:7035`

#### 4. Access the SPA Frontend

Open your browser and navigate to:  
`https://localhost:7035/spa`

---

## SPA Frontend

- The SPA is a static HTML/JavaScript application located at [`wwwroot/spa/index.html`](wwwroot/spa/index.html:1).
- **No build step is required**—the SPA is served directly by the backend.
- The SPA provides buttons to:
  - **Login:** Redirects to Azure AD login via the backend.
  - **Logout:** Logs out the user via the backend.
  - **Get Profile:** Fetches the authenticated user's profile.
  - **Check Auth:** Checks if the user is authenticated.

All frontend actions interact with backend endpoints under `/api/auth`.

---

## Azure AD App Registration

To configure Azure AD for this application, follow these steps:

1. **Register the Application:**
   - Go to the Azure portal and navigate to **Azure Active Directory** > **App registrations**.
   - Click **New registration**.
   - Provide a name for the application (e.g., `SPA-With-WebApi-Identity`).
   - Set the **Supported account types** to "Accounts in this organizational directory only" (or as per your requirements).
   - Add the following redirect URI: `https://localhost:7035/signin-oidc`.
   - Click **Register**.

2. **Configure API Permissions:**
   - Go to the **API permissions** section of the app registration.
   - Click **Add a permission**.
   - Select **Microsoft Graph**.
   - Add the following delegated permissions:
     - `openid`
     - `profile`
     - `email`
   - Click **Grant admin consent** to grant these permissions for all users in the directory.

3. **Define App Roles:**
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

4. **Configure Authentication:**
   - Go to the **Authentication** section of the app registration.
   - Add the following redirect URI: `https://localhost:7035/signin-oidc`.
   - Enable the following settings:
     - **ID tokens** (used for OpenID Connect authentication).

5. **Create a Client Secret:**
   - Go to the **Certificates & secrets** section of the app registration.
   - Click **New client secret**.
   - Provide a description and set an expiration period.
   - Copy the client secret value and store it securely. You will need this in the `appsettings.json` file.

6. **Update `appsettings.json`:**
   - Add the Azure AD configuration to your `appsettings.json` file as shown above.

---

## Role-Based Access Control (RBAC)

1. **Define Roles in Azure AD:**
   - Navigate to your Azure AD app registration.
   - Define roles like `Admin` and `User` in the **App Roles** section.
   - Assign these roles to users or groups.

2. **Enforce Roles in the Backend:**
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

3. **Debugging Claims:**
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

---

## Security Considerations

- **No Tokens in the Browser:** Tokens are managed entirely on the server, reducing the risk of token theft.
- **HTTP-Only Cookies:** Used for session management to prevent XSS attacks.
- **Role-Based Authorization:** Ensures that users can only access resources they are authorized for.

---

## Future Enhancements

- Add support for refresh tokens to extend session duration without requiring re-login.
- Implement additional logging and monitoring for security and performance.
- Add unit and integration tests for the backend.
