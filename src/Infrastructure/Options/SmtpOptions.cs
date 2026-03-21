namespace KindleEmailSenderBot.Infrastructure.Options;

public class SmtpOptions
{
    public const string ConfigurationKey = "Smtp";
    public string Server { get; init; } = string.Empty;
    public int Port { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}