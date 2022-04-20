using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpApiServer
{
    public class LogTimeExecutionRequestFilterAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable { get; } = false;

        IFilterMetadata IFilterFactory.CreateInstance(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<LogTimeExecutionRequestFilter>>();
            return new LogTimeExecutionRequestFilter(logger!);
        }
    }
}
