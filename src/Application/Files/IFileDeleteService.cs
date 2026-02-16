using KindleEmailSenderBot.Domain.Options;

namespace KindleEmailSenderBot.Application.Files;

public interface IFileDeleteService
{
    Task DeleteAsync(DeleteFileRequest? request = null);
}