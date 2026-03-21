using KindleEmailSenderBot.Domain;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KindleEmailSenderBot.Application.Files;

public class FileService : IFileService
{
    private readonly ITelegramBotClient botClient;
    private readonly FileServiceOptions options;

    public FileService(ITelegramBotClient botClient, IOptions<FileServiceOptions> options)
    {
        this.botClient = botClient;
        this.options = options.Value;
    }
    
    public Task DeleteAllAsync()
    {
        var path = Path.Combine(options.Path);
        
        var dirs = Directory.GetDirectories(path).ToList();

        foreach (var dir in dirs)
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
        
        return Task.CompletedTask;
    }
    
    public async Task<string> SaveAsync(DeliverFileRequest request)
    {
        var fileId = ValidationException.ThrowIfNull(request.FileId, "FileId is null");
        var chatId = ValidationException.ThrowIfNull(request.ChatId, "ChatId is null");
        var fileName = ValidationException.ThrowIfNull(request.FileName, "FileName is null");
            
            
        var file = await botClient.GetFile(fileId);
            
        var path = Path.Combine(options.Path, chatId.ToString() , DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            
            
        var filePath = Path.Combine(path, fileName);

        await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        await botClient.DownloadFile(file, fs);

        return filePath;
    }
}