using KindleEmailSenderBot.Application.Files;
using KindleEmailSenderBot.Domain.Users;

namespace KindleEmailSenderBot.Application.Chat;

public interface IChatService
{
    Task<User> GetOrCreateUserAsync(long chatId);
    Task UpdateEmailAsync(string mail, long chatId);
    Task<string> DeliverFileAsync(DeliverFileRequest request);
    Task<bool> CheckUserIsActiveAsync(long chatId);
    Task UpdateActivityAsync(bool activity, long chatId);
}