using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Input;
using KindleEmailSenderBot.TelegramBot.Commands.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KindleEmailSenderBot.TelegramBot;

public class TelegramCommandRouterService<T> where T : class
{
    private readonly T _controller;
    private ILogger<TelegramCommandRouterService<T>> _logger;

    public TelegramCommandRouterService(T controller, ILogger<TelegramCommandRouterService<T>> logger)
    {
        _controller = controller;
        _logger = logger;
    }

    public async Task<string?> Navigate(Update update)
    {
        try
        {
            MethodInfo? method = _controller.GetType().GetMethods()
                .FirstOrDefault(x =>
                {
                    var attribute =
                        x.GetCustomAttributes(typeof(MessageAttribute), false).FirstOrDefault() as MessageAttribute;
                    if (attribute == null) return false;

                    return (attribute.Message == update.Message?.Text || Regex.IsMatch(update.Message?.Text ?? "",
                               attribute.Pattern, RegexOptions.IgnoreCase))
                           && attribute.MessageType == update.Message?.Type;
                });

            if (method != null) return await (Task<string?>)method?.Invoke(_controller, [update])!;
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return "Something went wrong";
        }
    }
}