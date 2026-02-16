using KindleEmailSenderBot.Domain.Models;

namespace KindleEmailSenderBot.Application.BookBot;

public interface IBookBotManagementService
{
    Task<User> GetOrCreateUserAsync(long chatId);
    Task UpdateEmailAsync(string mail, long chatId);
    Task<string> DeliverFileAsync(string fileName, string fileId, long chatId);
    Task<bool> CheckUserIsActiveAsync(long chatId);
    Task UpdateActivityAsync(bool activity, long chatId);
}