using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Infrastructure;
using KindleEmailSenderBot.TelegramBot.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KindleEmailSenderBot.TelegramBot;

public class TelegramBotUpdateHandler : IUpdateHandler
{
    ILogger<TelegramBotUpdateHandler> _logger;
    IServiceProvider _serviceProvider;

    public TelegramBotUpdateHandler(ILogger<TelegramBotUpdateHandler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var router = scope.ServiceProvider.GetRequiredService<TelegramCommandRouterService<TelegramBotController>>();
        
        if (update.Type == UpdateType.Message && update.Message != null)
        {
            if(update.Message.Type == MessageType.Text) update.Message.Text = update.Message.Text.Trim();
            
            
            var response = await router.Navigate(update);
            if (response != null) await botClient.SendMessage(update.Message.Chat.Id, response, cancellationToken: cancellationToken);
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);
        return Task.CompletedTask;
    }
}