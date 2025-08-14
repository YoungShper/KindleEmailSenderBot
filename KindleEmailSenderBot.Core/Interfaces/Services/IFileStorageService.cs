using KindleEmailSenderBot.Core.Options;

namespace KindleEmailSenderBot.Core.Interfaces;

public interface IFileStorageService
{
    Task DeleteAsync(FileStorageContext? context = null);
}