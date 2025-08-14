using KindleEmailSenderBot.Core.Options;

namespace KindleEmailSenderBot.Core.Interfaces;

public interface IFileDownloadService
{
    Task<string> SaveAsync(DownloadContext context);
    
}