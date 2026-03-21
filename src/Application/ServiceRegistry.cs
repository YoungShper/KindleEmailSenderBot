using KindleEmailSenderBot.Application.Chat;
using KindleEmailSenderBot.Application.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KindleEmailSenderBot.Application;

public static class ServiceRegistry
{
    public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);
        services.AddFiles();
        services.AddChats();
        return services;
    }

    private static void AddFiles(this IServiceCollection services)
    {
        services.AddTransient<IFileService, FileService>();
    }

    private static void AddChats(this IServiceCollection services)
    {
        services.AddTransient<IChatService, ChatService>();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileServiceOptions>(configuration.GetSection(FileServiceOptions.ConfigurationKey));
    }
}