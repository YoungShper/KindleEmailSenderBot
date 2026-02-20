using KindleEmailSenderBot.Domain.Users;

namespace KindleEmailSenderBot.Application.Accounting;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(long chatId);
    public Task<bool> AddUserAsync(User user);
    public Task UpdateUserAsync(User user);
}