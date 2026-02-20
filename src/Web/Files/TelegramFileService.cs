using KindleEmailSenderBot.Application.Files;
using KindleEmailSenderBot.Domain.Options;
using KindleEmailSenderBot.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace KindleEmailSenderBot.Web.Files;

public class TelegramFileService : IFileService
{
    TelegramFileDownloadServiceOptions pathOptions;

    public TelegramFileService(IOptions<TelegramFileDownloadServiceOptions> localFileStorageSettings)
    {
        pathOptions = localFileStorageSettings.Value;
    }
    
    public async Task DeleteAsync(DeleteFileRequest? context)
    {
        var path = Path.Combine(pathOptions.Path);
        
        if (context is null)
        {
            var dirs = Directory.GetDirectories(path).ToList();
            await Parallel.ForEachAsync(dirs, new ParallelOptions  { MaxDegreeOfParallelism = 2}, async (dir, ct) =>
            {
                await Task.Run(() =>
                {
                    if (Directory.Exists(dir))
                        Directory.Delete(dir, true);
                }, ct);
            });
        }
        
        var concreteDir = Directory.GetDirectories(path).FirstOrDefault(x => x == context.ChatId);

        if (!string.IsNullOrEmpty(concreteDir) && Directory.Exists(Path.Combine(path, concreteDir)))
        {
            Directory.Delete(Path.Combine(path, concreteDir), true);
        }
    }
    
    public async Task<string> SaveAsync(DeliverFileRequest fileRequest)
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