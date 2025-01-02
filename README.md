# ASP.NET Core Bearer Authentication  
![Authentication Icon](https://miro.medium.com/v2/resize:fit:950/0*xnSBEMwTioRQQHgE.png)

## Overview  
Bearer Authentication is a token-based authentication mechanism used in ASP.NET Core applications. It involves transmitting a security token in the HTTP `Authorization` header to authenticate requests. Bearer tokens are commonly used in APIs to validate user identities and grant access to protected resources.

## Features  
- **Secure Token-Based Authentication:** Enables secure authentication without transmitting user credentials on every request.
- **Stateless Authentication:** Simplifies scalability by not requiring server-side session storage.
- **Standardized Protocol:** Compatible with OAuth 2.0 and other token-based systems.
- **Middleware Support:** Seamlessly integrates with ASP.NET Core's authentication pipeline.

## How It Works  
1. **Token Generation:** A token is issued to a user after successful authentication, typically via a login endpoint.
2. **Token Transmission:** The client includes the token in the `Authorization` header of subsequent requests.
3. **Token Validation:** The server validates the token to authenticate the user.
4. **Access Control:** The user is granted or denied access based on token claims.

## Example Implementation  

### Step 1: Install Required Packages  
Add the required NuGet packages for bearer authentication:
```bash
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
```

### Step 2: Configure Authentication  
Configure the authentication service in `Program.cs` or `Startup.cs`:
```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://your-issuer.com",
            ValidAudience = "https://your-audience.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"))
        };
    });
```

### Step 3: Add Middleware  
Ensure the authentication middleware is added to the pipeline:
```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Step 4: Protect Endpoints  
Use the `[Authorize]` attribute to secure API endpoints:
```csharp
[Authorize]
[HttpGet("/api/secure-endpoint")]
public IActionResult SecureEndpoint()
{
    return Ok("This is a secure endpoint!");
}
```

### Step 5: Token Issuance Example  
Implement a login endpoint to generate tokens:
```csharp
[AllowAnonymous]
[HttpPost("/api/login")]
public IActionResult Login([FromBody] LoginRequest loginRequest)
{
    if (loginRequest.Username == "user" && loginRequest.Password == "password")
    {
        var claims = new[] { new Claim(ClaimTypes.Name, loginRequest.Username) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "https://your-issuer.com",
            audience: "https://your-audience.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    return Unauthorized();
}
```

## Security Best Practices  
- **Use HTTPS:** Ensure all token transmissions are encrypted.
- **Secure Secrets:** Protect signing keys and other secrets using secure storage solutions.
- **Token Expiry:** Use short-lived tokens to minimize risk in case of compromise.
- **Refresh Tokens:** Implement refresh tokens for seamless user sessions.

## Additional Resources  
- [ASP.NET Core Authentication Overview](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)
- [JWT Bearer Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt)

---
### Contributions  
Contributions are welcome! Feel free to submit a pull request or open an issue.
