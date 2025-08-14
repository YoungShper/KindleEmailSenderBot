namespace KindleEmailSenderBot.Core.Interfaces;

public interface ISenderService
{
    Task SendFileAsync(string path, string to);
}