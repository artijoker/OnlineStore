using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpApiServer 
{
    public class BannedUserFilter : IAsyncAuthorizationFilter, IOrderedFilter
    {
        public int Order { get; } = 5;
        private readonly AccountService _service;

        public BannedUserFilter(AccountService service)
        {
            _service = service;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var claim = context.HttpContext.User.Claims.First(c => c.ValueType == ClaimTypes.NameIdentifier);
            var id = Guid.Parse(claim.Value);
            var account = await _service.GetById(id);

            if (account.IsBanned)
            {
                context.Result = new ObjectResult(new
                {
                    Succeeded = false,
                    Message = "Этот аккаунт заблокирован!"
                });
            }
        }
    }
}
