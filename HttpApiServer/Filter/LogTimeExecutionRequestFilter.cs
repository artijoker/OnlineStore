using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace HttpApiServer
{
    public class LogTimeExecutionRequestFilter : IResourceFilter
    {
        private readonly ILogger<LogTimeExecutionRequestFilter> _logger;
        private readonly Stopwatch _stopwatch;
        public LogTimeExecutionRequestFilter(ILogger<LogTimeExecutionRequestFilter> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _stopwatch.Start();
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _stopwatch.Stop();

            var time = _stopwatch.Elapsed;  
            var action = context.RouteData.Values["action"]!.ToString();
            _logger.LogInformation("Processing time action {action} {time}ms", action, time.TotalMilliseconds);
        }



        //public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        //{

        //    TimeSpan start = DateTime.Now.TimeOfDay;
        //    await next();
        //    TimeSpan end = DateTime.Now.TimeOfDay;
        //    var time = end - start;
        //    var action = context.RouteData.Values["action"]!.ToString();
        //    _logger.LogInformation("Processing time action {action} {time}ms", action, time.TotalMilliseconds);
        //}
    }
}
