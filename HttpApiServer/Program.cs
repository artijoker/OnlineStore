using Microsoft.EntityFrameworkCore;
using HttpApiServer;
using HttpModels;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EmailSenderLibrary;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Server Run");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var jwtConfig = builder.Configuration
        .GetSection("JwtConfig")
        .Get<JwtConfig>();

    const string dbPath = "myapp.db";

    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlite($"Data Source={dbPath};")
        );

    builder.Services.AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,

                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudiences = new[] 
                { 
                    jwtConfig.Audience 
                },
                ValidIssuer = jwtConfig.Issuer
            };
        });
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' " +
            "[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
            {
                new OpenApiSecurityScheme() {
                    Reference = new OpenApiReference() {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddAuthorization();

    builder.Services.AddSingleton<IClock, Clock>();
    builder.Services.AddSingleton<IPasswordHasher<Account>, PasswordHasher<Account>>();
    builder.Services.AddSingleton(jwtConfig);
    builder.Services.AddSingleton<ITokenService, TokenService>();

    builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEf>();

    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<ICartRepository, CartRepository>();
    builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
    builder.Services.AddScoped<IConfirmationCodeRepository, ConfirmationCodeRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();

    builder.Services.AddScoped<IConfirmationCodeGenerator, ConfirmationCodeGenerator>();

    builder.Services.AddScoped<CatalogService>();
    builder.Services.AddScoped<AccountService>();
    builder.Services.AddScoped<CartService>();
    builder.Services.AddScoped<OrderService>();


    builder.Services.AddCors();
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();
    builder.Services.Configure<SmtpCredentials>(builder.Configuration.GetSection("SmtpCredentials"));
    builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddControllers(options =>
    {
        //options.Filters.Add<ApiKeyFilter>();
        options.Filters.Add<ExceptionFilter>();
        //options.Filters.Add<BannedUserFilter>();
    });
    
    var app = builder.Build();

    //app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseMiddleware<UseBrowserCheck>();
    //app.UseMiddleware<UseRequestLoggingMiddleware>();
    //app.UseMiddleware<UseResponseLoggingMiddleware>();

    //app.Use(async (context, next) =>
    //{
    //    var userAgent = context.Request.Headers.UserAgent.ToString().ToLower();

    //    if (!userAgent.Contains("edg"))
    //    {
    //        await context.Response.WriteAsync("This site is browser only supported Edge");
    //        return;
    //    }
    //    await next();
    //});

    app.UseCors(policy =>
    {
        policy
         .AllowAnyMethod()
         .AllowAnyHeader()
         .WithOrigins("https://localhost:7139")
         .AllowCredentials();
    });

    app.MapControllers();
    app.MapGet("/", () => "Сервер работает");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal Error!");

}
finally
{
    Log.Information("Server Shutdown");
}

