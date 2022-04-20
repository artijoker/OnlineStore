using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels
{
    public class ConfirmationCodeModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = "";
    }
}
