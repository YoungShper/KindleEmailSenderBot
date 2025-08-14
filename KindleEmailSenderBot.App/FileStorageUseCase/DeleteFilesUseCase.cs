using KindleEmailSenderBot.Core.Interfaces;

namespace KindleEmailSenderBot.App.FileStorageUseCase;

public class DeleteFilesUseCase : IDeleteFilesUseCase
{
    IFileStorageService _fileStorageService;

    public DeleteFilesUseCase(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task DeleteFilesAsync()
    {
        await _fileStorageService.DeleteAsync();
    }
}