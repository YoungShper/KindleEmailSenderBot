using KindleEmailSenderBot.Application.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KindleEmailSenderBot.Web.BackgroundServices;

public class DeleteBackgroundService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public DeleteBackgroundService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var deleteFilesUseCase = scope.ServiceProvider.GetRequiredService<IFileService>();
            await deleteFilesUseCase.DeleteAllAsync();
            
            await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
        }
    }
}