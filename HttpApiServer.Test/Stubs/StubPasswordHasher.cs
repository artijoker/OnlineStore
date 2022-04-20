using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpApiServer.Test.Stubs
{
    public class StubPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public string HashPassword(TUser? user = null, string? password = null)
        {
            return "HashedPassword";
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            return hashedPassword == "HashedPassword" ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
