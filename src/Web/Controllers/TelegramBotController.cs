using System.Text;
using KindleEmailSenderBot.Application.BookBot;
using KindleEmailSenderBot.Web.Commands;
using KindleEmailSenderBot.Web.Commands.Attributes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KindleEmailSenderBot.Web.Controllers;

public class TelegramBotController
{
    IBookBotManagementService bookBotManagement;
    IMemoryCache chatState;
    KindleBotCommandProvider kindleBotCommandProvider;
    IConfiguration configuration;

    public TelegramBotController(
        IBookBotManagementService bookBotManagement, 
        IMemoryCache chatState, 
        KindleBotCommandProvider kindleBotCommandProvider, IConfiguration configuration)
    {
        this.bookBotManagement = bookBotManagement;
        this.chatState = chatState;
        this.kindleBotCommandProvider = kindleBotCommandProvider;
        this.configuration = configuration;
    }


    [Message("/start")]
    public async Task<string?> StartAsync(Update update)
    {
        if (!await bookBotManagement.CheckUserIsActiveAsync(update.Message!.Chat.Id))
        {
            var sb = new StringBuilder();
            var user = await bookBotManagement.GetOrCreateUserAsync(update.Message!.Chat.Id);
            sb.Append(await kindleBotCommandProvider.GetCommands(update.Message.Text));
            sb.Append(configuration["Smtp:Username"]);
            
            if(!string.IsNullOrEmpty(user.Email)) 
                sb.Append("\n\nYour current email address: " + user.Email);

            return sb.ToString();
        }
        return null;
    }
    
    [Message("/my_email")]
    public async Task<string?> GetEmailAsync(Update update)
    {
        if (await bookBotManagement.CheckUserIsActiveAsync(update.Message!.Chat.Id))
        {
            var user = await bookBotManagement.GetOrCreateUserAsync(update.Message!.Chat.Id);
            var result = await kindleBotCommandProvider.GetCommands(update.Message.Text);
            return  user.Email == string.Empty ? result + "not set" : result + user.Email;
        }
        return null;
    }
    
    [Message("/stop")]
    public async Task<string?> StopAsync(Update update)
    {
        if (await bookBotManagement.CheckUserIsActiveAsync(update.Message!.Chat.Id))
        {
            await bookBotManagement.UpdateActivityAsync(false, update.Message!.Chat.Id);
            chatState.Remove(update.Message.Chat.Id);
            
            return await kindleBotCommandProvider.GetCommands(update.Message.Text);
        }

        return null;
    }
    
    [Message("/help")]
    public async Task<string?> HelpAsync(Update update)
    {
        if (await bookBotManagement.CheckUserIsActiveAsync(update.Message!.Chat.Id) && !chatState.TryGetValue(update.Message!.Chat.Id, out _))
        {
            return await kindleBotCommandProvider.GetCommands(update.Message.Text) + configuration["Smtp:Username"];
        }

        return null;
    }
    
    [Message("/set_email")]
    public async Task<string?> SetMailAsync(Update update)
    {
        if (await bookBotManagement.CheckUserIsActiveAsync(update.Message!.Chat.Id) && !chatState.TryGetValue(update.Message.Chat.Id, out _))
        {
            chatState.Set(update.Message.Chat.Id, ChatState.WaitingForMail, DateTimeOffset.Now.AddHours(1));
            return await kindleBotCommandProvider.GetCommands(update.Message.Text);
        }

        return null;
    }
#if DEBUG
    [Message(pattern:@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    #else
    [Message(pattern:@"^[A-Z0-9._%+-]+@kindle\.com$")]
#endif
    public async Task<string?> SetMailAddressAsync(Update update)
    {
        if (await bookBotManagement.CheckUserIsActiveAsync(update.Message!.Chat.Id) && chatState.TryGetValue(update.Message.Chat.Id, out _))
        {
            string email = update.Message.Text;
            
            chatState.Remove(update.Message.Chat.Id);
            var success = await bookBotManagement.UpdateEmailAsync(email, update.Message.Chat.Id);
            if (success) return "Your Email address successfully set.";
        }
        return null;
    }
    
    [Message(MessageType = MessageType.Document)]
    public async Task<string> SendFileAsync(Update update)
    {
        var mail = await bookBotManagement.DeliverFileAsync(update.Message.Document.FileName, update.Message.Document.FileId, update.Message.Chat.Id);
        return $"File sent to {mail}";
    }
}