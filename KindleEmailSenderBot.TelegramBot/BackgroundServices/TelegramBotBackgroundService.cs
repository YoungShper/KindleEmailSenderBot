using KindleEmailSenderBot.TelegramBot.Commands;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace KindleEmailSenderBot.TelegramBot.BackgroundServices;

public class TelegramBotBackgroundService : BackgroundService
{
    readonly ITelegramBotClient _telegramBotClient;
    readonly IUpdateHandler _updateHandler;
    readonly List<BotCommand> _kindleBotCommandList;

    public TelegramBotBackgroundService(ITelegramBotClient telegramBotClient, IUpdateHandler updateHandler, KindleBotCommandProvider kindleBotCommandProvider)
    {
        _telegramBotClient = telegramBotClient;
        _updateHandler = updateHandler;
        _kindleBotCommandList = kindleBotCommandProvider.GetCommands().Select(x => x.BotCommand).ToList();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _telegramBotClient.StartReceiving(_updateHandler, cancellationToken: stoppingToken);
            await _telegramBotClient.SetMyCommands(_kindleBotCommandList, cancellationToken: stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }
}