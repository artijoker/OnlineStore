using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels
{
    public class LogInResponse : ResponseModel<Account>
    {
        public string Token { get; set; } = "";
    }
}
