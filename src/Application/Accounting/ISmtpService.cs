namespace KindleEmailSenderBot.Web.Services;

public interface ISmtpService
{
    Task SendFileAsync(string path, string to);
}