using KindleEmailSenderBot.Application.Accounting;
using KindleEmailSenderBot.Application.Files;
using KindleEmailSenderBot.Domain.Models;
using KindleEmailSenderBot.Domain.Options;

namespace KindleEmailSenderBot.Application.BookBot;

public class BookBotManagementService : IBookBotManagementService
{
    private readonly IUserRepository userRepository;
    private readonly IFileDownloadService fileDownloadService;
    private readonly IFileSenderService fileSenderService;

    public BookBotManagementService(IUserRepository userRepository, IFileDownloadService fileDownloadService, IFileSenderService fileSenderService)
    {
        this.userRepository = userRepository;
        this.fileDownloadService = fileDownloadService;
        this.fileSenderService = fileSenderService;
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

    public async Task<string> DeliverFileAsync(string fileName, string fileId, long chatId)
    {
        var user = await userRepository.GetByIdAsync(chatId);
        
        if (user == null)
        {
            throw new NullReferenceException("Пользователь не найден");
        }
        
        var data = new DownloadFileRequest(fileName, chatId.ToString(), fileId);
        var path = await fileDownloadService.SaveAsync(data);
        await fileSenderService.SendFileAsync(path, user.Email);
        return user.Email;
    }

    public async Task<bool> CheckUserIsActiveAsync(long chatId)
    {
        var user = await userRepository.GetByIdAsync(chatId);
        if (user != null) return user.IsActive;

        return false;
    }
}