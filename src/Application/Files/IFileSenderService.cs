namespace KindleEmailSenderBot.Application.Files;

public interface IFileSenderService
{
    Task SendFileAsync(string path, string to);
}