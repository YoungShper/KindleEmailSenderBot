using KindleEmailSenderBot.Core.Models;

namespace KindleEmailSenderBot.Core.Interfaces;

public interface IUserRepository
{
    public Task<UserModel?> GetUserAsync(long chatId);
    public Task<bool> AddUserAsync(UserModel user);
    public Task<bool> UpdateUserAsync(UserModel user);
}