using KindleEmailSenderBot.Domain.Options;

namespace KindleEmailSenderBot.Application.Files;

public interface IFileService
{
    Task DeleteAsync(DeleteFileRequest? request = null);
    Task<string> SaveAsync(DeliverFileRequest fileRequest);
}