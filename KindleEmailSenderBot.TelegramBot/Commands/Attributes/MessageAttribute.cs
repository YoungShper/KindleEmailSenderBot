using Telegram.Bot.Types.Enums;

namespace KindleEmailSenderBot.TelegramBot.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class MessageAttribute : Attribute
{
    public string? Message { get; set; }
    public string? Pattern { get; set; }
    public MessageType MessageType { get; set; }
    
    public MessageAttribute(string? message = null, string? pattern = "^$", MessageType messageType = MessageType.Text)
    {
        Message = message;
        MessageType = messageType;
        Pattern = pattern;
    }
}