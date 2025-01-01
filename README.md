# ASP.NET Core Logging  
![Logging Icon](https://doumer.me/wp-content/uploads/2023/05/Structured-Logging-Techniques-in-ASP.net-core-.png)

## Overview  
Logging is an essential aspect of modern application development. This guide provides an overview of how ASP.NET Core handles logging, including:  
- Understanding Log Levels.  
- Log Providers.
- Best practices for production and development environments.  
- Practical examples of structured logging.  

---

## Log Levels  

ASP.NET Core logging categorizes log messages into different levels of severity to help developers manage and filter logs effectively. Below are the log levels in ascending order of severity:  

1. **Trace**  
   - Most detailed logs.  
   - Used for diagnostics and troubleshooting during development.  

2. **Debug**  
   - Informational messages for debugging.  
   - Typically used in development environments.  

3. **Information**  
   - General operational events.  
   - Represents the flow of the application.  

4. **Warning**  
   - Highlights issues that could potentially become errors.  
   - Useful for preemptive problem detection.  

5. **Error**  
   - Indicates failures that affect specific functionality.  
   - Does not halt the application but requires attention.  

6. **Critical**  
   - Severe errors causing complete application failure or requiring immediate attention.  

---

## Log Providers

ASP.NET Core supports various logging providers to handle log output. Below are some commonly used log providers:

1. **Console**  
   - Outputs logs to the console, ideal for development and debugging environments.

2. **Debug**  
   - Sends log messages to the debug output window. Useful for debugging applications locally.

3. **EventLog**  
   - Writes logs to the Windows Event Log. Suitable for monitoring production systems running on Windows.

4. **TraceSource**  
   - Integrates with `System.Diagnostics.TraceSource` to provide flexible trace logging.

5. **Third-Party Providers**  
   - Includes popular providers like **Serilog**, **NLog**, and **Log4Net** for advanced and customized logging needs.

Example:

```csharp
// Program.cs

builder.Services.AddLogging(cfg =>
{
    cfg.AddDebug();
});

```
---

## Best Practices  

- **Environment-Based Logging:**  
  - In **production**, start logging from the **Information** level and above to avoid excessive log volume.  
  - Use **Debug** and **Trace** levels only in development environments to diagnose issues effectively.  

- **Focus on Critical Levels for Clarity:**  
  - For better readability, ensure critical and error logs are concise and meaningful.  
  - Avoid logging unnecessary data to reduce noise and improve performance.  

## Configuring Logging Levels and Providers

### Specify Default and Category-Specific Logging Levels

The following configuration ensures that messages from the **Information** level are logged while ignoring lower levels, such as **Debug**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

- **Important:** In the above configuration, `_logger.LogDebug("Text")` will not execute as it falls below the **Information** level.
- **Note:** You can specify logging levels for specific categories, such as `"Microsoft.AspNetCore": "Warning"`.

### Specify Log Levels for Specific Providers

You can configure different log levels for each provider. For example, setting a default log level of **Error** globally while setting **Trace** for the console provider:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Warning"
    },
    "Console": {
      "LogLevel": {
        "Default": "Trace"
      }
    }
  }
}
```

## Practical Examples  

Here’s how to implement structured logging in ASP.NET Core:  

### Configuring Logging in `appsettings.json`  

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Error"
    }
  }
}
```

### Adding Logging in Code  

```csharp
using Microsoft.Extensions.Logging;

public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Fetching weather forecasts.");
        try
        {
            // Application logic here
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching weather forecasts.");
            throw;
        }
    }
}
```

### Structured Logging with Serilog  

Serilog is a popular third-party logging provider for ASP.NET Core.  

#### Install Serilog  

```bash
dotnet add package Serilog.AspNetCore
```

#### Configure Serilog  

```csharp
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

var app = builder.Build();
app.Run();
```

---

## Additional Resources  

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)  
- [Serilog Documentation](https://serilog.net/)  

---  

Feel free to suggest improvements or raise issues if you find areas for enhancement. ??  

---
