using EmailSenderLibrary;
using HttpModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpApiServer
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionFilter(IHostEnvironment hostEnvironment) =>
            _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
            var message = TryGetMessageFromException(context);
            if (message is not null)
            {
                context.Result = new ObjectResult(
                    new
                    {
                        Succeeded = false,
                        Message = message
                    });
                context.ExceptionHandled = true;
            }

        }


        private static string? TryGetMessageFromException(ExceptionContext context)
        {
            return context.Exception switch
            {
                DuplicateEmailException => "Пользователь с таким email уже существует",
                DuplicateLoginException => "Пользователь с таким логином уже существует",
                InvalidLoginException => "Неверный логин",
                InvalidPasswordException => "Неверный пароль",
                NetworkException => "Не удалось отправить код подтверждения. Попробуйте зайти позже.",
                UnidentifiedConfirmationCodeException => "Неопознанный код подтверждения",
                InvalidConfirmationCodeException => "Неверный код подтверждения",
                EmptyСartException => "Пустая корзина",
                ItemNotFoundCartException => "Товар не найден в корзине",
                _ => null
            };
        }
    }
}

