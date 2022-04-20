using HttpModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpApiServer
{
    public class ApiKeyFilter : IAuthorizationFilter, IOrderedFilter
    {
        public int Order { get; } = 0;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.Any(h => h.Key == "apikey" && h.Value == "7254"))
            {
                context.Result = new ObjectResult(
                    new NotFoundApiKeyResponse() { 
                        Succeeded = false,
                        Message = "В заголовках не указан API ключ"
                    }
                );
            }
        }
    }
}
