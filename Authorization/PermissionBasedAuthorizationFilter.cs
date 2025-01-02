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
