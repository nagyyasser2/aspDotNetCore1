# ASP.NET Core Configuration Guide (Episode 2) `Options Pattern` .
![Configuration Icon](https://static.vecteezy.com/system/resources/previews/040/190/651/non_2x/configuration-and-setting-icon-concept-vector.jpg)

---

## Overview
This guide explains how ASP.NET Core handles configurations, including:
- Reading configurations with **options pattern** .
- Practical examples
---

## Steps to implement options pattern .

### 1. create your configuration section.

### 2. create class to refer to the configuration section.

### 3. map config section to class

### Example : 

- create configuration section :

  ```json
   // appsettings.json

   {
    "AllowedExtentions": "jpg,png",
    "MaxSizeInMegaBytes": 1,
    "EnableCompression":  true
  }
  ```
- create the maping class :

     ```csharp
     // AttachmentOptions.cs

    namespace aspDotNetCore.Config
    {
        public class AttachmentOptions
        {
            public string AllowedExtentions { get; set; }
            public int MaxSizeInMegaBytes { get; set; }
            public bool EnableCompression { get; set; }
        }
    }
     ```
- mapping, we have three types of mapping :

## 1.
```csharp
// Program.cs

var attachmentOptions = builder.Configuration.GetSection("AttachmentOptions").Get<AttachmentOptions>();

builder.Services.AddSingleton<attachmentOptions>();
```
## 2.
```csharp
// Program.cs

var attachmentOptions = new AttachmentOptions();
builder.Configuration.GetSection("AttachmentOptions").Bind(attachmentOptions);
builder.Services.AddSingleton(attachmentOptions);

```
## 3. `options-pattern` .
```csharp
// Program.cs

builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));
```

>[!IMPORTANT]
> options-pattern allow us to inject `IOptions` interface which is <ins>Singleton</ins>\,
> and `IOptionsSnapshot` which is <ins>Scoped</ins> it reload per each request and reads the values when accessing the <ins>Value</ins> property,\
> and `IOptionsMonitor` which is <ins>Singleton</ins> it reload when update happen

Example :

```csharp
// ConfigController.cs 

using aspDotNetCore.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace aspDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AttachmentOptions> _attachmentOptions;

        public ConfigController(IConfiguration configuration, IOptions<AttachmentOptions> attachmentOptions)
        {
            this._configuration = configuration;
            this._attachmentOptions = attachmentOptions;
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
                SigningKey = _configuration["SigningKey"],
                AttachmentOptions = _attachmentOptions.Value,
            };
            return Ok(config);
        }
    }
}

```


>[!NOTE]
> The mapping to be sucess the class must have public default parameter-less constructor .

Happy Coding! ??
