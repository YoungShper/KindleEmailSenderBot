using KindleEmailSenderBot.App.BookBot;
using KindleEmailSenderBot.App.FileStorageUseCase;
using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.DataAccess.Postgres.Context;
using KindleEmailSenderBot.DataAccess.Postgres.Repositories;
using KindleEmailSenderBot.Infrastructure;
using KindleEmailSenderBot.Infrastructure.Options;
using KindleEmailSenderBot.Infrastructure.Services;
using KindleEmailSenderBot.TelegramBot.BackgroundServices;
using KindleEmailSenderBot.TelegramBot.Commands;
using KindleEmailSenderBot.TelegramBot.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace KindleEmailSenderBot.TelegramBot;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json");
        var configuration = builder.Configuration;
        builder.Services.AddLogging(o => o.AddConsole());
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IUpdateHandler, TelegramBotUpdateHandler>();
        builder.Services.Configure<CommandsOptions>(builder.Configuration.GetSection(CommandsOptions.XmlDataPath));
        builder.Services.AddSingleton<KindleBotCommandProvider>();
        builder.Services.AddSingleton<ITelegramBotClient>(o =>
        {
            var token = configuration["Telegram:Token"] ?? string.Empty;
            var client = new TelegramBotClient(token);
            return client;
        });
        builder.Services.AddHostedService<TelegramBotBackgroundService>();
        builder.Services.Configure<LocalFileStorageOptions>(builder.Configuration
            .GetSection(LocalFileStorageOptions.WorkDir));
        builder.Services.AddScoped<IFileDownloadService, TelegramFileDownloadService>();
        builder.Services.AddScoped<IFileDeleteService, TelegramFileDeleteService>();
        builder.Services.AddScoped<IDeleteFilesUseCase, DeleteFilesUseCase>();
        builder.Services.AddDbContext<KindleDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString(nameof(KindleDbContext)));
        });
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.Smtp));
        builder.Services.AddSingleton<SmtpClientFactory>();
        builder.Services.AddScoped<IFileSenderService, SmtpFileSenderService>();
        builder.Services.AddScoped<IBookBotManagementService, BookBotManagementService>();
        builder.Services.AddScoped<TelegramBotController>();
        builder.Services.AddScoped<TelegramCommandRouterService<TelegramBotController>>();
        
        builder.Services.AddHostedService<DeleteBackgroundService>();
        
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<KindleDbContext>();
            dbContext.Database.Migrate();
        }
        app.Run();
    }
}