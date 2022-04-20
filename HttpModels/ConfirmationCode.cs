using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels
{
    public class ConfirmationCode : IEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Code { get; set; } = "";
    }
}
