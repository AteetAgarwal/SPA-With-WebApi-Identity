# SPA-With-WebApi-Identity

This project demonstrates a secure authentication and authorization flow for a Single Page Application (SPA) interacting with a backend Web API. The application avoids storing access tokens in the browser by relying on server-side token management and HTTP-only cookies.

## Structure

- **Frontend**: The SPA implementation. See the [frontend README](./frontend/README.md) for setup and details.
- **Backend**: The Web API implementation. See the [backend README](./backend/README.md) for setup and details.

## Key Features

- **OAuth 2.0 Authorization Code Flow**: Secure authentication using Azure AD.
- **Role-Based Access Control (RBAC)**: Access to resources is restricted based on user roles.
- **Secure Token Management**: Tokens are managed on the server, ensuring security.

## Setup Instructions

1. Follow the instructions in the [backend README](./backend/README.md) to configure and run the Web API.
2. Follow the instructions in the [frontend README](./frontend/README.md) to configure and run the SPA.
