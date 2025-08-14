namespace KindleEmailSenderBot.Infrastructure.Options;

public class SmtpOptions
{
    public const string Smtp = "Smtp";
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}