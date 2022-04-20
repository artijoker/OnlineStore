using Bogus;
using EmailSenderLibrary;
using HttpApiClient;
using HttpApiServer.Test.Stubs;
using HttpModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HttpApiServer.Test
{
    public class AccountControllerIntegrationTest : IDisposable
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private ITokenService _tokenService;
        private IPasswordHasher<Account> _hasher;
        private IEmailSender _emailSender;
        private IConfirmationCodeGenerator _codeGenerator;

        public AccountControllerIntegrationTest(ITestOutputHelper output)
        {

            _applicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureTestServices(services =>
                {
                    //const string dbPath = "testDB.db";
                    //services.AddDbContext<AppDbContext>(
                    //    options => options.UseSqlite($"Data Source={dbPath}"));
                    services.RemoveAll<ITokenService>();
                    services.AddSingleton<ITokenService, StubTokenService>();

                    services.RemoveAll<IPasswordHasher<Account>>();
                    services.AddSingleton<IPasswordHasher<Account>, StubPasswordHasher<Account>>();

                    services.RemoveAll<IEmailSender>();
                    services.AddScoped<IEmailSender, StubEmailSender>();

                    services.RemoveAll<IConfirmationCodeGenerator>();
                    services.AddScoped<IConfirmationCodeGenerator, StubConfirmationCodeGenerator>();

                    using var serviceProvider = services.BuildServiceProvider();
                    using var serviceScope = serviceProvider.CreateScope();
                    using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                    context!.Database.EnsureCreated();
                });
                builder.UseSerilog((_, config) =>
                    config.WriteTo.TestOutput(output));
            });
            _tokenService = new StubTokenService();
            _emailSender = new StubEmailSender();
            _codeGenerator = new StubConfirmationCodeGenerator();
            _hasher = new StubPasswordHasher<Account>();
        }

        [Fact]
        public async Task User_successfully_registered()
        {
            using var serviceScope = _applicationFactory.Services.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            var client = new ShopClient(httpClient: _applicationFactory.CreateClient());
            AccountRegistrationModel registrationModel = GetFakeAccount().Generate();
            await client.Registration(registrationModel);

            AccountLogInModel logInModel = new()
            {
                Login = registrationModel.Login,
                Password = registrationModel.Password
            };
            var response = await client.LogIn(logInModel);
            
            Assert.True(response!.Succeeded);
        }

        [Fact]
        public async Task User_with_this_email_already_exists()
        {
            var client = new ShopClient(httpClient: _applicationFactory.CreateClient());

            var model = GetFakeAccount().Generate();

            await client.Registration(model);
            var response = await client.Registration(model);
            Assert.False(response!.Succeeded);
            Assert.Equal("Пользователь с таким email уже существует", response.Message);
        }

        [Fact]
        public async Task User_with_this_login_already_exists()
        {
            var client = new ShopClient(httpClient: _applicationFactory.CreateClient());

            var model = GetFakeAccount().Generate();
            await client.Registration(model);

            model.Email = GetFakeAccount().Generate().Email;
            var response = await client.Registration(model);
            Assert.False(response!.Succeeded);
            Assert.Equal("Пользователь с таким логином уже существует", response.Message);
        }

        [Fact]
        public async Task Two_factor_authentication_was_successful()
        {
            var client = new ShopClient(httpClient: _applicationFactory.CreateClient());

            AccountRegistrationModel registrationModel = GetFakeAccount().Generate();
            await client.Registration(registrationModel);
            
            AccountLogInModel logInModel = new()
            {
                Login = registrationModel.Login,
                Password = registrationModel.Password
            };

            var confirmationCodeResponse = await client.LogIn(logInModel);

            ConfirmationCodeModel codeModel = new()
            {
                Id = confirmationCodeResponse!.Result!.Id,
                Code = _codeGenerator.GenerateCode(6)
            };

            var logInResponse = await client.СonfirmСode(codeModel);

            var token = _tokenService.GenerateToken(new());
            Assert.True(logInResponse!.Succeeded);
            Assert.Equal(token, logInResponse.Token);
            Assert.Equal(registrationModel.Email, logInResponse.Result!.Email);
            Assert.Equal(registrationModel.Login, logInResponse.Result!.Login);
            Assert.Equal(_hasher.HashPassword(new(), ""), logInResponse.Result!.PasswordHash);
        }

        public void Dispose()
        {
            using (var serviceScope = _applicationFactory.Server.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context!.Database.EnsureDeleted();
            }
            _applicationFactory.Dispose();
        }

        private static Faker<AccountRegistrationModel> GetFakeAccount()
        {
            return new Faker<AccountRegistrationModel>()
               .RuleFor(it => it.Email, f => f.Internet.Email())
               .RuleFor(it => it.Login, f => f.Internet.UserName())
               .RuleFor(it => it.Password, f => f.Internet.Password());
        }
    }
}
