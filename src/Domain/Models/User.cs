using System.ComponentModel.DataAnnotations;

namespace KindleEmailSenderBot.Domain.Models;

public class User
{
    public string Email { get; private set; }
    public long ChatId { get; private set; }
    public bool IsActive { get; private set; }

    public User(
        string email, 
        long chatId, 
        bool isActive)
    {
        Email = email;
        ChatId = chatId;
        IsActive = isActive;
    }
    public void ChangeActivity(bool isActive)
    {
        IsActive = isActive;
    }
    
    public void SetEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ValidationException("Email должен быть заполнен");
        }
        Email = email;
    }
}