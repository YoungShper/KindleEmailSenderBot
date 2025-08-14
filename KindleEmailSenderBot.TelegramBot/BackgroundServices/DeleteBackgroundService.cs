using KindleEmailSenderBot.App.FileStorageUseCase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KindleEmailSenderBot.TelegramBot.BackgroundServices;

public class DeleteBackgroundService : BackgroundService
{
    IServiceProvider _serviceProvider;

    public DeleteBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            if (now.Hour == 23 && now.Minute == 59 && now.Second == 59)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var deleteFilesUseCase = scope.ServiceProvider.GetRequiredService<IDeleteFilesUseCase>();
                    await deleteFilesUseCase.DeleteFilesAsync();
                }
                
            }
        }
    }
}