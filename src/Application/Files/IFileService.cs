namespace KindleEmailSenderBot.Application.Files;

public interface IFileService
{
    Task DeleteAllAsync();
    Task<string> SaveAsync(DeliverFileRequest request);
}