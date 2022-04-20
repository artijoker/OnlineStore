using EmailSenderLibrary;
using HttpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HttpApiServer;

[Route("account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [TypeFilter(typeof(LogRequestParameters))]
    [HttpPost("registration")]
    public async Task<ActionResult<LogInResponse>> Register(AccountRegistrationModel model)
    {
        var(account, token) = await _service.RegisterUser(model.Email, model.Login, model.Password);
        return new LogInResponse() { 
            Succeeded = true,
            Result = account,
            Token = token
        };
    }

    [Authorize]
    [HttpGet("get_account")]
    public async Task<ActionResult<ResponseModel<Account>>> GetAccount()
    {
        var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return new ResponseModel<Account>()
        {
            Succeeded = true,
            Result = await _service.GetById(id)
        };
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet("get_all_accounts")]
    public async Task<ActionResult<ResponseModel<IReadOnlyList<Account>>>> GetAllAccounts()
    {
        return new ResponseModel<IReadOnlyList<Account>>()
        {
            Succeeded = true,
            Result = await _service.GetAll()
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<ResponseModel<ConfirmationCodeModel>>> LogIn(AccountLogInModel accountModel)
    {
        var codeId = await _service.Authorize(accountModel.Login, accountModel.Password);
        var model = new ConfirmationCodeModel()
        {
            Id = codeId
        };
        return new ResponseModel<ConfirmationCodeModel>()
        {
            Succeeded = true,
            Result = model
        };
    }

    [HttpPost("confirm_code")]
    public async Task<ActionResult<LogInResponse>> СonfirmСode(ConfirmationCodeModel model)
    {
        var (account, token) = await _service.СonfirmСode(model.Id, model.Code);

        return new LogInResponse()
        {
            Succeeded = true,
            Result = account,
            Token = token
        };
    }
}