using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpApiServer.Test.Stubs
{
    internal class StubConfirmationCodeGenerator : IConfirmationCodeGenerator
    {
        public string GenerateCode(int length = 6)
        {
            return "123456";
        }
    }
}
