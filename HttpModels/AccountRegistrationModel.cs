using System.ComponentModel.DataAnnotations;

namespace HttpModels;

public class AccountRegistrationModel
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
}


