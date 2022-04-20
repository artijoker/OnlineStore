using HttpModels;

namespace HttpApiServer;

public class UseBrowserCheck
{
    private readonly RequestDelegate _next;

    public UseBrowserCheck(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userAgent = context.Request.Headers.UserAgent.ToString().ToLower();

        if (!userAgent.Contains("edg"))
        {
            await context.Response.WriteAsJsonAsync(
                new
                {
                    Succeeded = false,
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "Сайт поддерживает только браузер Edge"
                });
            return;
        }
        await _next(context);
    }
}