# Keycloak .NET Core Integration

A .NET Core Web API project demonstrating integration with Keycloak for authentication and authorization. This project showcases how to implement secure authentication in a .NET Core application using Keycloak as the identity provider.

## Features

- 🔐 JWT-based authentication using Keycloak
- 👥 Role-based authorization
- 🚀 Swagger UI integration with JWT authentication support
- 🛡️ Protected and public API endpoints
- 🎯 Token generation endpoint
- ⚙️ Configurable Keycloak settings

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Keycloak Server](https://www.keycloak.org/) (running locally or remotely)
- IDE of your choice (Visual Studio, VS Code, etc.)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/keycloak-dotnet-core.git
   cd keycloak-dotnet-core
   ```

2. Configure Keycloak:
   - Start your Keycloak server (default: http://localhost:8080)
   - Log in to the Keycloak Admin Console
   - Create a new realm named "demo-realm"
   - Create a new client:
     - Client ID: "demo-client"
     - Client Protocol: "openid-connect"
     - Access Type: "confidential"
     - Service Accounts Enabled: "ON"
     - Valid Redirect URIs: "http://localhost:5012/*"
     - Web Origins: "*"
   - After saving the client, go to the "Credentials" tab to get the client secret
   - Create roles (e.g., "admin", "user")
   - Create test users and assign roles

3. Update the Keycloak configuration in `appsettings.json`:
   ```json
   "Keycloak": {
     "Enabled": true,
     "Authority": "http://localhost:8080/realms/demo-realm",
     "Audience": "demo-client",
     "Realm": "demo-realm",
     "ClientId": "demo-client",
     "ClientSecret": "demo-secret-key-should-be-replaced"
   }
   ```
   Replace "demo-secret-key-should-be-replaced" with the actual client secret from Keycloak.

4. Run the application:
   ```bash
   dotnet run
   ```

5. Access the Swagger UI at `http://localhost:5012/swagger`

## API Endpoints

### Public Endpoints
- `GET /api/Auth/public` - Accessible without authentication
- `GET /api/Auth/config` - Returns Keycloak configuration

### Protected Endpoints
- `GET /api/Auth/protected` - Requires authentication
- `GET /api/Auth/admin` - Requires admin role
- `POST /api/Token/generate` - Generates JWT token

## Authentication Flow

1. Users authenticate through Keycloak
2. The application validates JWT tokens
3. Protected endpoints check for valid authentication
4. Role-based endpoints verify user permissions

## Security Features

- JWT token validation
- Role-based access control
- Configurable token validation parameters
- HTTPS support
- Secure token handling

## Development

The project uses:
- .NET 8.0
- Microsoft.AspNetCore.Authentication.JwtBearer (7.0.2)
- Swashbuckle.AspNetCore (6.6.2)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- [Keycloak](https://www.keycloak.org/) - Open Source Identity and Access Management
- [.NET Core](https://dotnet.microsoft.com/) - Free, Cross-platform, Open source developer platform

## Testing the Setup

1. Start the Keycloak server:
   ```bash
   # Using Docker
   docker run -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:latest start-dev
   ```

2. Run the .NET Core application:
   ```bash
   dotnet run
   ```

3. Test the endpoints:
   - Open Swagger UI: http://localhost:5012/swagger
   - Try the public endpoint first
   - Generate a token using the Token endpoint
   - Use the token to access protected endpoints

## Common Issues

- If you get a connection refused error, make sure Keycloak is running and accessible
- If token validation fails, verify that:
  - The realm name matches exactly
  - The client secret is correct
  - The Authority URL is correct and accessible
  - The client has the correct roles and permissions configured