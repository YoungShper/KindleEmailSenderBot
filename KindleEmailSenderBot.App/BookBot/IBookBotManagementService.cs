using KindleEmailSenderBot.Core.Models;

namespace KindleEmailSenderBot.App.BookBot;

public interface IBookBotManagementService
{
    Task NotifyUsersAsync(string message);
    Task<UserModel> GetOrCreateUserAsync(long chatId);
    Task<bool> UpdateEmailAsync(string mail, long chatId);
    Task DeliverFileAsync(string fileName, string fileId, long chatId);
    Task<bool> CheckUserIsActiveAsync(long chatId);
    Task<bool> UpdateActivityAsync(bool activity, long chatId);
}