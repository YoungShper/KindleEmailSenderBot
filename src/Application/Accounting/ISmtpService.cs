namespace KindleEmailSenderBot.Application.Accounting;

public interface ISmtpService
{
    Task SendFileAsync(string path, string to);
}