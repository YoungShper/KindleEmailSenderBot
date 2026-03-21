using KindleEmailSenderBot.Domain.Users;

namespace KindleEmailSenderBot.Application.Accounting;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(long chatId);
    public Task<bool> AddAsync(User user);
    public Task UpdateAsync(User user);
}