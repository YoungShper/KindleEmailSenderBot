using KindleEmailSenderBot.Web.Commands;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace KindleEmailSenderBot.Web.BackgroundServices;

public class TelegramBotBackgroundService : BackgroundService
{
    private readonly ITelegramBotClient telegramBotClient;
    private readonly IUpdateHandler updateHandler;
    private readonly List<BotCommand> kindleBotCommandList;

    public TelegramBotBackgroundService(ITelegramBotClient telegramBotClient, IUpdateHandler updateHandler, KindleBotCommandProvider kindleBotCommandProvider)
    {
        this.telegramBotClient = telegramBotClient;
        this.updateHandler = updateHandler;
        kindleBotCommandList = kindleBotCommandProvider.GetCommands().Select(x => x.BotCommand).ToList();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            telegramBotClient.StartReceiving(updateHandler, cancellationToken: stoppingToken);
            await telegramBotClient.SetMyCommands(kindleBotCommandList, cancellationToken: stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }
}