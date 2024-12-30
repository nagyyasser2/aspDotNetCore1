# ASP.NET Core Action Filters

Action Filters in ASP.NET Core API are used to execute custom logic before or after an action method runs, enabling tasks like logging, authentication, or data transformation.

---

### Key Characteristics of Action Filters

- **Action-Specific Execution:** Action filters execute custom logic before or after the execution of an action method or its result.
- **Cross-Cutting Concerns:** Ideal for handling cross-cutting concerns such as logging, validation, and error handling specific to controller actions.
- **Execution Order:** Filters are executed in a specific order based on their registration, with global filters running first, followed by controller-level and then action-level filters.
- **Short-Circuiting:** Filters can modify or terminate the request/response pipeline by skipping the action method or altering the result.

---

![ActionFilter Image](https://umbracare.net/media/jirailjj/asp-net-core-filters-pipeline.jpg?rmode=pad&width=1200&height=630&bgcolor=fff&v=1da493c363b43d0)

---

## How to Create an Action Filter?

Creating a custom action filter in ASP.NET Core involves defining a class that processes HTTP requests in the pipeline. Below are the steps to create a simple `LogActivityFilter`.

---

### Step 1: Create the ActionFilter Class

1. Create a folder named `Filters` in your project (if not already created).
2. Inside the `Filters` folder, create a file called `LogActivityFilter.cs` and define the Action Filter logic.

```csharp
// LogActivityFilter.cs
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspDotNetCore.Filters
{
    public class LogActivityFilter : IActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing Action {context.ActionDescriptor.DisplayName} on Controller {context.Controller}, With Args {context.ActionArguments}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} Finished on Controller {context.Controller}");
        }
    }
}
```

---

### Step 2: Register the ActionFilter in `Program.cs`

To enable the action filter in your application, register it in the pipeline. Open your `Program.cs` file and add the following line:

```csharp
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<LogActivityFilter>();
});
```

> **Note:** The above registration is a **global** registration, meaning it will apply to all controllers and actions.

---

### Step 3: Apply the Filter at the Controller or Action Level (Optional)

If you prefer to apply the filter to a specific controller or action instead of globally, you can use the `[ServiceFilter]` or `[TypeFilter]` attribute.

```csharp
[ServiceFilter(typeof(LogActivityFilter))]
public class ExampleController : ControllerBase
{
    [HttpGet]
    public IActionResult GetExample()
    {
        return Ok("Action Filter Example");
    }
}
```

---

### Anohter Example About Action Filter Specific On Controller Or Action Method

### Step 1: Create the `LogSensitiveActionAttribute.cs` Class

1. Create a folder named `Filters` in your project (if not already created).
2. Inside the `Filters` folder, create a file called `LogSensitiveActionAttribute.cs` and define the Action Filter logic.

```csharp
//LogSensitiveActionAttribute.cs

using Microsoft.AspNetCore.Mvc.Filters;

namespace aspDotNetCore.Filters
{
    public class LogSensitiveActionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Sentetive Action Executed...");
        }
    }
}

```
---

### Step 2: Register the ActionFilter On `Contorller`

```csharp
 [Route("api/[controller]")]
    [ApiController]
    [LogSensitiveAction]
    public class ProductsController : ControllerBase
    { }
```
---

### Step 3: Register the ActionFilter On `Action` .

```csharp
// Get a product by ID

[HttpGet]
[Route("{key}")]
[LogSensitiveAction]
public ActionResult<Product> GetProduct([FromRoute(Name = "key")] int id)
{
    _logger.LogDebug("trying  to get product with id#{id} ", id);
    var product = _dbContext.Set<Product>().Find(id);
    if (product == null)
    {
        _logger.LogWarning("Product #{id}, Not Found!", id);
        return NotFound($"Product with ID {id} not found.");
    }

    return Ok(product);
}
```
---

> **Note:** We Can Still Register The ActionAttribute In `Program.cs` .

```csharp
// Program.cs 

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<LogActivityFilter>();
    opt.Filters.Add<LogSensitiveActionAttribute>();
});
```




---

## Key Differences Between Action Filters and Middleware

| Feature                    | Action Filters                                                                 | Middleware                                                                                     |
|----------------------------|--------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| **Scope**                  | Specific to controller actions.                                                | Applies to the entire HTTP request/response pipeline.                                         |
| **Customization**          | Can access detailed action-level metadata.                                     | Can modify `HttpRequest` and `HttpResponse` objects.                                          |
| **Use Cases**              | Logging, validation, caching, and error handling for specific controllers.     | Authentication, CORS, custom request/response handling, and general-purpose processing tasks. |

---

## Additional Notes

- **Execution Pipeline:** Filters are a part of the ASP.NET Core request pipeline and can work alongside middleware.
- **Other Filter Types:** ASP.NET Core provides additional filter types, such as `ExceptionFilter`, `AuthorizationFilter`, and `ResultFilter`, for handling different stages of the request/response lifecycle.
- **Reusable Components:** Action Filters can be reused across multiple projects to handle common concerns.

---

## References

- [Microsoft Docs - Filters in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters)
- [Action Filters Explained](https://www.tutorialsteacher.com/core/action-filters-in-aspnet-core)

---

Happy coding! ??

