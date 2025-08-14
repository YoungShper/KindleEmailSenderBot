namespace KindleEmailSenderBot.Infrastructure;

public class LocalFileStorageOptions
{
    public const string WorkDir = "WorkDir";
    public string Path { get; set; } = string.Empty;
}