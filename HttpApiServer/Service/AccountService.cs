using EmailSenderLibrary;
using HttpModels;
using Microsoft.AspNetCore.Identity;

namespace HttpApiServer;

public class AccountService
{
    private readonly IUnitOfWork _unit;
    private readonly IPasswordHasher<Account> _hasher;
    private readonly ITokenService _tokenService;
    private readonly IEmailSender _emailSender;
    private readonly IConfirmationCodeGenerator _confirmationCodeGenerator;

    public AccountService(IUnitOfWork unit, 
        IPasswordHasher<Account> hasher, 
        ITokenService tokenService, 
        IEmailSender emailSender,
        IConfirmationCodeGenerator confirmationCodeGenerator)
    {
        _unit = unit;
        _hasher = hasher;
        _tokenService = tokenService;
        _emailSender = emailSender;
        _confirmationCodeGenerator = confirmationCodeGenerator;
    }

    public async Task<(Account, string)> RegisterUser(string email, string login, string password)
    {
        if (await _unit.AccountRepository.IsEmailExist(email))
            throw new DuplicateEmailException();

        if (await _unit.AccountRepository.IsLoginExist(login))
            throw new DuplicateLoginException();

        Account account = new()
        {
            Id = Guid.NewGuid(),
            Login = login,
            Email = email,
            Role = Role.User
        };

        var hashedPassword = _hasher.HashPassword(account, password);
        account.PasswordHash = hashedPassword;

        var cart = new Cart() 
        { 
            Id = Guid.NewGuid(), 
            AccountId = account.Id, 
            CartItems = new List<CartItem>() 
        };

        await _unit.AccountRepository.Add(account);
        await _unit.CartRepository.Add(cart);
        await _unit.SaveChangesAsync();

        return (account, _tokenService.GenerateToken(account));
    }

    public async Task<Guid> Authorize(string login, string password)
    {
        var account = await _unit.AccountRepository.FindByLogin(login);
        if (account == null)
            throw new InvalidLoginException();

        var result = _hasher.VerifyHashedPassword(
            account, account.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed)
            throw new InvalidPasswordException();

        var confirmationCode = new ConfirmationCode()
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Code = _confirmationCodeGenerator.GenerateCode(6)
        };
        ;
        await _emailSender.SendMessage(account.Email, "Код подтверждения", confirmationCode.Code);
        ;
        await _unit.ConfirmationCodeRepositiry.Add(confirmationCode);
        await _unit.SaveChangesAsync();
        ;
        return confirmationCode.Id;
    }

    public async Task<(Account, string)> СonfirmСode(Guid codeId, string code)
    {
        ConfirmationCode confirmationCode;
        try
        {
            confirmationCode = await _unit.ConfirmationCodeRepositiry.GetById(codeId); ;
        }
        catch (InvalidOperationException)
        {
            throw new UnidentifiedConfirmationCodeException();
        }

        if (confirmationCode.Code != code)
            throw new InvalidConfirmationCodeException();

        var account = await _unit.AccountRepository.GetById(confirmationCode.AccountId);
        return (account, _tokenService.GenerateToken(account));
    }

    public Task<bool> IsEmailExist(string email)
    {
        return _unit.AccountRepository.IsEmailExist(email);
    }

    public Task<bool> IsLoginExist(string login)
    {
        return _unit.AccountRepository.IsLoginExist(login);
    }

    public async Task<Account> GetById(Guid Id)
    {
        return await _unit.AccountRepository.GetById(Id);
    }

    public async Task<IReadOnlyList<Account>> GetAll()
    {
        return await _unit.AccountRepository.GetAll();
    }
}