# ASP.NET Core Configuration Guide (Episode 1)

![Configuration Icon](https://static.vecteezy.com/system/resources/previews/040/190/651/non_2x/configuration-and-setting-icon-concept-vector.jpg)

---

## Overview
This guide explains how ASP.NET Core handles configurations, including:
- Configuration sources
- Reading configurations
- Predefined sources
- Practical examples
- Adding custom configuration files

---

## How ASP.NET Core Finds Configuration Sources

The `Configuration` property retrieves all sources where configuration data is stored:

```csharp
builder.Configuration.Sources
```
You can modify this collection to add or remove configuration sources as needed.

---

## Reading Configuration Values
To read configuration values, use the following:

```csharp
builder.Configuration["ConnectionStrings:DefaultConnection"]
```

For connection strings, ASP.NET Core provides a specific helper method:

```csharp
builder.Configuration.GetConnectionString("DefaultConnection")
```

---

## Predefined Configuration Sources
ASP.NET Core supports the following built-in configuration sources:

1. **JSON Files:** `appsettings.json`, `appsettings.{Environment}.json`
2. **Environment Variables**
3. **User Secrets** (for local development)

---

## Practical Example: Reading Configuration in a Controller
Follow these steps to create a simple example:

1. Create a new controller named `ConfigController.cs`.
2. Inject the `IConfiguration` interface to access configuration values.

### Sample Code
```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult GetConfig()
        {
            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                ConnectionStrings = _configuration.GetConnectionString("DefaultConnection"),
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"],
                SigningKey = _configuration["SigningKey"]
            };

            return Ok(config);
        }
    }
}
```
### Notes
- All configuration values retrieved using this approach are returned as strings.

---

## Configuration Overrides
1. **`appsettings.{Environment}.json`** overrides `appsettings.json` based on:
   - `ASPNETCORE_ENVIRONMENT` environment variable
   - `launchSettings.json`
   - User secrets (in development)

2. **Environment Variables** override `appsettings.json` and `appsettings.{Environment}.json` values.

---

## Adding a Custom JSON Configuration File
To add a custom JSON file:

1. Create the file, e.g., `Config.json`.
2. Add the following line in `Program.cs`:

```csharp
builder.Configuration.AddJsonFile("Config.json");
```

---

## Adding User Secrets
- open **cmd** in the root directory :
```bash
dotnet  user-secrets init
```
 example output: 
```bash
Set UserSecretsId to '766edc58-0dc5-48e7-8464-2db7bc2ebfee' for MSBuild project 'D:\NAGY_YASSER\asp.net core applications\aspDotNetCore\aspDotNetCore.csproj'.
```
## To Set User Secrets :
```bash
dotnet user-secrets set "Signingkey" "anydasldfslafd"
```
>[!NOTE]
> user-secrets is just a file away from your source code.
## Summary
This guide covered the basics of ASP.NET Core configurations, including predefined sources, reading configurations, overrides, and adding custom configuration files. For further details, consult the [official ASP.NET Core documentation](https://learn.microsoft.com/aspnet/core).

---

Happy Coding! ??
