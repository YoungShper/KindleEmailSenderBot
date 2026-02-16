namespace KindleEmailSenderBot.Infrastructure.Options;

public class TelegramFileDownloadServiceOptions
{
    public const string WorkDir = "WorkDir";
    public string Path { get; set; } = string.Empty;
}