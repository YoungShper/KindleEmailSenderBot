using KindleEmailSenderBot.Application.Files;
using KindleEmailSenderBot.Domain.Options;
using KindleEmailSenderBot.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KindleEmailSenderBot.Web.File;

public class TelegramFileDownloadService : IFileDownloadService
{
    private readonly ITelegramBotClient botClient;
    private readonly IOptions<TelegramFileDownloadServiceOptions> options;
    private readonly ILogger<TelegramFileDownloadService> logger;
    

    public TelegramFileDownloadService(ILogger<TelegramFileDownloadService> logger, 
        ITelegramBotClient botClient, IOptions<TelegramFileDownloadServiceOptions> options)
    {
        this.botClient = botClient;
        this.options = options;
        this.logger = logger;
    }

    public async Task<string> SaveAsync(DownloadFileRequest fileRequest)
    {
        if(fileRequest.FileId == null || fileRequest.ChatId == null || fileRequest.FileName == null) 
            throw new NullReferenceException("Empty context");
            
            
        var file = await botClient.GetFile(fileRequest.FileId);
            
        var path = Path.Combine(options.Value.Path, fileRequest.ChatId, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            
            
        var filePath = Path.Combine(path, fileRequest.FileName);

        await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        await botClient.DownloadFile(file, fs);

        return filePath;
    }
}