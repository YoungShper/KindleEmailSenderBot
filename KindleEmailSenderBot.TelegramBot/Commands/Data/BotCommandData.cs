using Telegram.Bot.Types;

namespace KindleEmailSenderBot.TelegramBot.Commands.Data;

public class BotCommandData
{
    public string CommandName { get; set; }
    public BotCommand BotCommand { get; set; }
    public string Response { get; set; }
}