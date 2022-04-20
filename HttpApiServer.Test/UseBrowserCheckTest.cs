using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;
using HttpModels;
namespace HttpApiServer.Test;

public class UseBrowserCheckTest
{
    [Fact]
    public async void UseBrowserCheck_request_from_Edge_browser_was_successful()
    {
        bool isNext = false;

        var useBrowserCheck = new UseBrowserCheck(
            new RequestDelegate(
                context =>
                {
                    isNext = true;
                    return Task.CompletedTask;
                })
            );
        var httpContext = new DefaultHttpContext();
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.88 Safari/537.36 Edg/99.0.1150.36";
        httpContext.Request.Headers.UserAgent = userAgent;
        await useBrowserCheck.InvokeAsync(httpContext);

        Assert.True(isNext);
    }

    [Fact]
    public async void UseBrowserCheck_request_not_from_Edge_browser_is_blocked()
    {
        bool isNext = false;

        var useBrowserCheck = new UseBrowserCheck(
            new RequestDelegate(
                context =>
                {
                    isNext = true;
                    return Task.CompletedTask;
                })
            );

        var httpContext = new DefaultHttpContext();
        using MemoryStream stream = new();
        httpContext.Response.Body = stream;

        await useBrowserCheck.InvokeAsync(httpContext);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ResponseModel<object>>(body, options: new() { PropertyNameCaseInsensitive = true });
        
        Assert.False(isNext);
        Assert.False(response!.Succeeded);
        Assert.Equal(response!.StatusCode, StatusCodes.Status403Forbidden);
        Assert.Equal("Сайт поддерживает только браузер Edge", response!.Message);
    }
}





