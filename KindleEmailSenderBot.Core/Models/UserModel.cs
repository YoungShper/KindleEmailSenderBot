namespace KindleEmailSenderBot.Core.Models;

public class UserModel
{
    public required string Email { get; set; }
    public required long ChatId { get; set; }
    public required bool IsActive { get; set; }
}