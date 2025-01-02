# ASP.NET Core Permission-based Authorization

![Permission](https://www.tenfold-security.com/wp-content/uploads/NTFS-vs-Share-Permissions-Example-2.png)

## Overview  
Permission-based authorization in ASP.NET Core allows you to control access to resources based on fine-grained permissions assigned to users or roles. This approach provides greater flexibility compared to role-based authorization by enabling a more detailed and scalable access control system.

## Features  
- **Granular Access Control:** Define specific permissions for various actions and resources.
- **Dynamic Policy Management:** Add or update permissions dynamically without redeploying the application.
- **Extensibility:** Easily integrate with custom user or role management systems.
- **Secure Middleware Integration:** Built on ASP.NET Core's robust authentication and authorization pipeline.

## How It Works  
1. **Define Permissions:** Create a centralized list of permissions in your application.
2. **Assign Permissions:** Associate permissions with users or roles in your database.
3. **Validate Permissions:** Use custom authorization handlers to enforce permission checks.
4. **Protect Resources:** Apply policies to controllers, actions, or endpoints based on required permissions.

## Example Implementation  

### Step 1: Define Permissions  enum
Create a permissions class to store all permissions in a centralized location:
```csharp
namespace aspDotNetCore.Data
{
    public enum Permission
    {
        ReadProducts = 1,
        AddProducts,
        EditProducts,
        DeleteProducts
    }
}

```

### Step 2: Create UserPermission class  
```csharp
namespace aspDotNetCore.Data
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public Permission  PermissionId { get; set; }
    }
}
```

### Step 3: Create PermissionAttribute  

```csharp
using aspDotNetCore.Data;

namespace aspDotNetCore.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttribute: Attribute
    {
        public CheckPermissionAttribute(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; }
    }
}

```

### Step 4: Apply Attribute To Method  
Add claims to users representing their permissions:
```csharp
[HttpGet]
[Route("")]
[CheckPermission(Permission.ReadProducts)]
public ActionResult<IEnumerable<Product>> GetProducts()
{
    var products = _dbContext.Set<Product>().ToList();
    return Ok(products);
}
```

### Step 5: Create Authorizaton Filter 

```csharp
using aspDotNetCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace aspDotNetCore.Authorization
{
    public class PermissionBasedAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ApplicationDbContext dbContext;

        public PermissionBasedAuthorizationFilter(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attributes = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is CheckPermissionAttribute);
            if (attributes != null)
            {
                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var hasPermisssion = dbContext.Set<UserPermission>().Any(x => x.UserId == userId && x.PermissionId == attributes.Permission);
                    if (!hasPermisssion)
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }

        }
    }
}
```

### Step 6: Register the filter :
```csharp
// Program.cs

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<LogActivityFilter>();
    opt.Filters.Add<PermissionBasedAuthorizationFilter>();
});
```

## Security Best Practices  
- **Use HTTPS:** Always secure your application with HTTPS.
- **Validate Claims:** Ensure claims are added securely and verified properly.
- **Use Database Storage:** Store permissions and associations in a secure database.
- **Follow Least Privilege:** Assign only necessary permissions to users or roles.

## Additional Resources  
- [ASP.NET Core Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/)
- [Custom Authorization Handlers](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies#custom-authorization-handlers)

---
### Contributions  
Contributions are welcome! Feel free to submit a pull request or open an issue.
