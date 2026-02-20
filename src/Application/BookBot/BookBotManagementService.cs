using KindleEmailSenderBot.Application.Accounting;
using KindleEmailSenderBot.Application.Files;
using KindleEmailSenderBot.Domain;
using KindleEmailSenderBot.Domain.Options;
using KindleEmailSenderBot.Domain.Users;
using KindleEmailSenderBot.Web.Services;

namespace KindleEmailSenderBot.Application.BookBot;

public class BookBotManagementService : IBookBotManagementService
{
    private readonly IUserRepository userRepository;
    private readonly IFileService fileService;
    private readonly ISmtpService smtpService;

    public BookBotManagementService(
        IUserRepository userRepository, 
        IFileService fileService, 
        ISmtpService smtpService)
    {
        this.userRepository = userRepository;
        this.fileService = fileService;
        this.smtpService = smtpService;
    }

    public async Task<User> GetOrCreateUserAsync(long chatId)
    {
        var user = await userRepository.GetByIdAsync(chatId);

        if (user == null)
        {
            user = new User(string.Empty, chatId, true);
            await userRepository.AddUserAsync(user);
        }
        else
        {
            user.ChangeActivity(true);
            await userRepository.UpdateUserAsync(user);
        }

        return user;
    }

    public async Task UpdateEmailAsync(string mail, long chatId)
    {
        var user = await userRepository.GetByIdAsync(chatId);

        if (user == null)
        {
            throw new NullReferenceException("Пользователь не найден");
        }
        
        user.SetEmail(mail);
        await userRepository.UpdateUserAsync(user);
    }
    public async Task UpdateActivityAsync(bool activity, long chatId)
    {
        var user = await userRepository.GetByIdAsync(chatId);
        user.ChangeActivity(activity);
        await userRepository.UpdateUserAsync(user);
    }

    public async Task<string> DeliverFileAsync(DeliverFileRequest request)
    {
        var fileName = ValidationException.ThrowIfNull(request.FileName, "Имя файла должно быть задано");
        var chatId = ValidationException.ThrowIfNull(request.ChatId, "ChatId должен быть заполнен");
        
        var user = await userRepository.GetByIdAsync(chatId);
        
        if (user == null)
        {
            throw new InvalidOperationException("Пользователь не найден");
        }
        
        var path = await fileService.SaveAsync(request);
        await smtpService.SendFileAsync(path, user.Email);
        return user.Email;
    }

    public async Task<bool> CheckUserIsActiveAsync(long chatId)
    {
        var user = await userRepository.GetByIdAsync(chatId);
        if (user != null) return user.IsActive;

        return false;
    }
}