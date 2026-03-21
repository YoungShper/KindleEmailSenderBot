namespace KindleEmailSenderBot.Application.Files;

public class FileServiceOptions
{
    public const string ConfigurationKey = "WorkDir";
    public string Path { get; set; } = string.Empty;
}