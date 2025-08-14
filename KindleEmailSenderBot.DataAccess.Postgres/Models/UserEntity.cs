namespace KindleEmailSenderBot.DataAccess.Postgres.Models;

public class UserEntity
{
    public string Email { get; set; } = string.Empty;
    public long ChatId { get; set; }
    public bool IsActive { get; set; }
}