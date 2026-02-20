using KindleEmailSenderBot.Domain.Options;
using KindleEmailSenderBot.Domain.Users;

namespace KindleEmailSenderBot.Application.BookBot;

public interface IBookBotManagementService
{
    Task<User> GetOrCreateUserAsync(long chatId);
    Task UpdateEmailAsync(string mail, long chatId);
    Task<string> DeliverFileAsync(DeliverFileRequest request);
    Task<bool> CheckUserIsActiveAsync(long chatId);
    Task UpdateActivityAsync(bool activity, long chatId);
}