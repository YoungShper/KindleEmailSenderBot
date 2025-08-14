using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KindleEmailSenderBot.Infrastructure.Services;

public class TelegramDownloadFileService : IFileDownloadService
{
    ILogger<TelegramDownloadFileService> _logger;
    ITelegramBotClient _botClient;
    LocalFileStorageOptions _pathOptions;
    

    public TelegramDownloadFileService(ILogger<TelegramDownloadFileService> logger, 
        ITelegramBotClient botClient, IOptions<LocalFileStorageOptions> localFileStorageSettings)
    {
        _logger = logger;
        _botClient = botClient;
        _pathOptions = localFileStorageSettings.Value;
    }

    public async Task<string> SaveAsync(DownloadContext context)
    {
        if(context.FileId == null || context.ChatId == null || context.FileName == null) 
            throw new NullReferenceException("Empty context");
            
            
        var file = await _botClient.GetFile(context.FileId);
            
        var path = Path.Combine(_pathOptions.Path, context.ChatId, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            
            
        var filePath = Path.Combine(path, context.FileName);

        await using var ms = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        await _botClient.DownloadFile(file, ms);

        return filePath;
    }
}