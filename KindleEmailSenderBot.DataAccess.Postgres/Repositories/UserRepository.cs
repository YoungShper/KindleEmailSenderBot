using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Core.Models;
using KindleEmailSenderBot.DataAccess.Postgres.Context;
using KindleEmailSenderBot.DataAccess.Postgres.Mappers;
using Microsoft.EntityFrameworkCore;

namespace KindleEmailSenderBot.DataAccess.Postgres.Repositories;

public class UserRepository : IUserRepository
{
    private KindleDbContext _context;

    public UserRepository(KindleDbContext context)
    {
        _context = context;
    }

    public async Task<UserModel?> GetUserAsync(long chatId)
    {
       var result = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ChatId == chatId);
       return result.ToUserModel();
    }

    public async Task<bool> AddUserAsync(UserModel user)
    {
        await _context.Users.AddAsync(user.ToUserEntity());
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateUserAsync(UserModel user)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
        if (data == null) return false;
        data.Email = user.Email;
        data.IsActive = user.IsActive;
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}