using KindleEmailSenderBot.Domain.Options;

namespace KindleEmailSenderBot.Application.Files;

public interface IFileDownloadService
{
    Task<string> SaveAsync(DownloadFileRequest fileRequest);
}