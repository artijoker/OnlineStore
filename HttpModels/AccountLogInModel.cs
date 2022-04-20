using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels
{
    public class AccountLogInModel
    {
        [Required]
        public string Login { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
