using KindleEmailSenderBot.Data.Configurations;
using KindleEmailSenderBot.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace KindleEmailSenderBot.Data.Context;

public class KindleDbContext(DbContextOptions<KindleDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}