namespace KindleEmailSenderBot.Core.Interfaces;

public interface IFileSenderService
{
    Task SendFileAsync(string path, string to);
}