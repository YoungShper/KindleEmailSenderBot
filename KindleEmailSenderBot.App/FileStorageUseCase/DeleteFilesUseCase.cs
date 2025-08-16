using KindleEmailSenderBot.Core.Interfaces;

namespace KindleEmailSenderBot.App.FileStorageUseCase;

public class DeleteFilesUseCase : IDeleteFilesUseCase
{
    IFileDeleteService _fileDeleteService;

    public DeleteFilesUseCase(IFileDeleteService fileDeleteService)
    {
        _fileDeleteService = fileDeleteService;
    }

    public async Task DeleteFilesAsync()
    {
        await _fileDeleteService.DeleteAsync();
    }
}