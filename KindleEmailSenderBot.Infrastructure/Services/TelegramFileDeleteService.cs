using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KindleEmailSenderBot.Infrastructure;

public class TelegramFileDeleteService : IFileDeleteService
{
    LocalFileStorageOptions _pathOptions;

    public TelegramFileDeleteService(IOptions<LocalFileStorageOptions> localFileStorageSettings)
    {
        _pathOptions = localFileStorageSettings.Value;
    }
    
    public async Task DeleteAsync(FileStorageContext context)
    {
        var path = Path.Combine(_pathOptions.Path);
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
}