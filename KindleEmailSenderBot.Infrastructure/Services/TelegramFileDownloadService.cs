using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KindleEmailSenderBot.Infrastructure.Services;

public class TelegramFileDownloadService : IFileDownloadService
{
    ITelegramBotClient _botClient;
    LocalFileStorageOptions _pathOptions;
    

    public TelegramFileDownloadService(ILogger<TelegramFileDownloadService> logger, 
        ITelegramBotClient botClient, IOptions<LocalFileStorageOptions> localFileStorageSettings)
    {
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

        await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        await _botClient.DownloadFile(file, fs);

        return filePath;
    }
}