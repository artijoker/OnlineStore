using EmailSenderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpApiServer.Test.Stubs
{
    public class StubEmailSender : IEmailSender
    {
        public Task SendMessage(string? toEmail = null, string? subject = null, string? body = null, CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }
}
