using KindleEmailSenderBot.DataAccess.Postgres.Configurations;
using KindleEmailSenderBot.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace KindleEmailSenderBot.DataAccess.Postgres.Context;

public class KindleDbContext(DbContextOptions<KindleDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}