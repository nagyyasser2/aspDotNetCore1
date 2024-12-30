# ASP.NET Core Middleware

Middleware in ASP.NET Core is a modular component in the request-response pipeline, handling HTTP requests and responses in a sequential manner. Middleware can perform a wide range of tasks such as authentication, logging, request modification, response generation, and routing.

### Key Characteristics of Middleware:
- **Request Processing:** Middleware can process requests and generate responses or pass the request to the next middleware.
- **Execution Order:** Middleware components are executed in the order they are configured.
- **Short-circuiting:** Middleware can terminate further processing in the pipeline based on certain conditions.

This modular approach allows for flexible and efficient management of HTTP traffic in ASP.NET Core applications.

![Middleware Image](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTz5FqtfhjU3yO_Zyp4TXRLS2OJQfIzO4A8eK5zBKF8JcTSyQtWbuxvw0WszhwpHsoSRoM)

---

## How to Create a Custom Middleware

Creating a custom middleware in ASP.NET Core involves defining a class that processes HTTP requests in the pipeline. Below are the steps to create a simple profiling middleware.

### Step 1: Create the Middleware Class

1. Create a folder named `Middlewares` in your project (if not already created).
2. Inside the `Middlewares` folder, create a file called `ProfilingMiddleware.cs` and define the middleware logic.

```csharp
using System.Diagnostics;

namespace aspDotNetCore.Middlewares
{
    public class ProfilingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfilingMiddleware> _logger;

        // Constructor for injecting dependencies
        public ProfilingMiddleware(RequestDelegate next, ILogger<ProfilingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // The Invoke method is called for every HTTP request
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            finally
            {
                // Log the time taken for the request processing
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                _logger.LogInformation("Request for {Path} took {ElapsedMilliseconds}ms", context.Request.Path, elapsedMilliseconds);
            }
        }
    }
}
```
### Step 2: Register the Middleware in `Program.cs`

To enable the middleware in your application, you need to register it in the pipeline. Open your `Program.cs` file and add the following line:

```csharp
app.UseMiddleware<ProfilingMiddleware>();
```
This will ensure that your middleware is executed for every HTTP request.

## Instructions for Creating Custom Middleware

Follow these steps to create your own custom middleware:

1. Create a Middleware Class:

    - Define a class like `ProfilingMiddleware`. This class should not implement any other interfaces such as `IActionFilter` or `IExceptionFilter`.


2. Inject Dependencies:

    - Inject the `RequestDelegate` in the middleware constructor. The `RequestDelegate` is used to pass the request to the next middleware in the pipeline.

3. Implement the Invoke Method:

    - The `Invoke` method receives the `HttpContext` and processes the request. You can execute any custom logic here (e.g., logging, performance profiling, authentication, etc.).

4. Register the Middleware:

    - Add the middleware in the desired order within the `Program.cs` file using `app.UseMiddleware<YourMiddlewareClass>().` The order in which you register the middleware determines the order of execution in the pipeline.



## Additional Notes

 - Middleware is executed in the order it is registered. Be mindful of the sequence, especially when dealing with cross-cutting concerns like authentication, logging, and exception handling.
 - Middleware can modify both the `HttpRequest` and `HttpResponse` objects, making it an ideal place for custom request processing tasks.



  



























