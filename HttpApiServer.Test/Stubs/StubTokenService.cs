using HttpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpApiServer.Test.Stubs
{
    public class StubTokenService : ITokenService
    {
        public string GenerateToken(Account? account = null)
        {
            return "TestToken";
        }
    }
}
