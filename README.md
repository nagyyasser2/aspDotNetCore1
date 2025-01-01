# ASP.NET Core Basic Authentication  
![Authentication Icon](https://secumantra.com/wp-content/uploads/2024/02/basic-authentication.jpg)

## Overview  
Basic Authentication in ASP.NET Core is a simple and effective way to secure APIs and web applications. It involves transmitting user credentials (username and password) in the HTTP header of requests. While straightforward, it should be used with HTTPS to ensure data is encrypted during transmission.

## Features  
- **Lightweight Security:** Ideal for lightweight and stateless authentication.
- **Easy Integration:** Simple to implement in any ASP.NET Core project.
- **Custom Authentication Schemes:** Supports custom implementations to tailor authentication to application needs.
- **Middleware Integration:** Easily integrates with ASP.NET Core middleware.

## How It Works  
1. **User Credentials:** The client sends a username and password encoded in Base64 in the `Authorization` header of the HTTP request.
2. **Validation:** The server decodes and validates the credentials against a datastore (e.g., a database or an in-memory list).
3. **Response:** Upon successful validation, the server processes the request; otherwise, it returns a `401 Unauthorized` status.

## Example Implementation  
### Step 1: Add Authentication Service  
In `Program.cs` or `Startup.cs`, configure Basic Authentication.
```csharp
builder.Services.AddAuthentication().
    AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

```

### Step 2: Create Authentication Handler  
Create a custom `BasicAuthenticationHandler`.
```csharp
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace aspDotNetCore.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Ensure Authorization header exists
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header not found");

            string username;
            string password;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials[0];
                password = credentials[1];
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (!ValidateCredentials(username, password))
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private bool ValidateCredentials(string username, string password)
        {
            // Replace this with actual logic to validate the username and password
            return username == "admin" && password == "password";
        }
    }
}

```

### Step 3: Add Authorization Middleware  
Ensure the middleware is added to the pipeline.
```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Step 4: Secure Endpoints  
Use the `[Authorize]` attribute to protect your API endpoints.
```csharp
[Authorize]
[HttpGet("/api/secure-endpoint")]
public IActionResult SecureEndpoint()
{
    return Ok("This is a secure endpoint!");
}
```

## Security Considerations  
- **Use HTTPS:** Always use HTTPS to encrypt credentials during transmission.
- **Avoid Storing Passwords:** Store hashed and salted passwords instead of plaintext.
- **Token-Based Alternatives:** For more robust security, consider using token-based authentication methods such as JWT.
- **Rate Limiting:** Implement rate limiting to mitigate brute-force attacks.

## Additional Resources  
- [ASP.NET Core Authentication Overview](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)
- [Authentication Middleware](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/middleware)

---
### Contributions  
Contributions are welcome! Feel free to submit a pull request or open an issue.

