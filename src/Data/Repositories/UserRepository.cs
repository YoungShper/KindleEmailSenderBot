using KindleEmailSenderBot.Application.Accounting;
using KindleEmailSenderBot.Data.Context;
using KindleEmailSenderBot.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KindleEmailSenderBot.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly KindleDbContext context;

    public UserRepository(KindleDbContext context)
    {
        this.context = context;
    }

    public async Task<User?> GetByIdAsync(long chatId)
    {
       var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ChatId == chatId);
       return user;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        var result = await context.SaveChangesAsync();
        return result > 0;
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Update(user);
        await context.SaveChangesAsync();
    }
}