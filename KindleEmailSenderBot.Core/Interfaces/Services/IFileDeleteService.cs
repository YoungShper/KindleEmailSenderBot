using KindleEmailSenderBot.Core.Options;

namespace KindleEmailSenderBot.Core.Interfaces;

public interface IFileDeleteService
{
    Task DeleteAsync(FileDeleteContext? context = null);
}