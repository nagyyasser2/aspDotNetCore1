using Microsoft.AspNetCore.Mvc.Filters;

namespace aspDotNetCore.Filters
{
    public class LogActivityFilter: IActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            this._logger = logger;
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
