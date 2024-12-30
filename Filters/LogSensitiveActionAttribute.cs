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
