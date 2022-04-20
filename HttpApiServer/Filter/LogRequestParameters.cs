using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpApiServer
{
    public class LogRequestParameters : Attribute, IActionFilter
    {
        private readonly ILogger<LogRequestParameters> _logger;

        public LogRequestParameters(ILogger<LogRequestParameters> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var (key, value) in context.ActionArguments)
            {
                _logger.LogInformation("[{Endpoint}] {Param}: {@Value}",
                    context.ActionDescriptor.DisplayName, key, value);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

