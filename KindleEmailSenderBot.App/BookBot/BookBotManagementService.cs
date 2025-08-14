using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Core.Models;
using KindleEmailSenderBot.Core.Options;

namespace KindleEmailSenderBot.App.BookBot;

public class BookBotManagementService : IBookBotManagementService
{
    private readonly IUserRepository _userRepository;
    private readonly IFileDownloadService _fileDownloadService;
    private readonly ISenderService _senderService;

    public BookBotManagementService(IUserRepository userRepository, IFileDownloadService fileDownloadService, ISenderService senderService)
    {
        _userRepository = userRepository;
        _fileDownloadService = fileDownloadService;
        _senderService = senderService;
    }
    public Task NotifyUsersAsync(string message)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel> GetOrCreateUserAsync(long chatId)
    {
        var user = await _userRepository.GetUserAsync(chatId);

        if (user == null)
        {
            user = new UserModel
            {
                ChatId = chatId,
                IsActive = true,
                Email = ""
            };
            await _userRepository.AddUserAsync(user);
        }
        else
        {
            user.IsActive = true;
            _userRepository.UpdateUserAsync(user);
        }

        return user;
    }

    public async Task<bool> UpdateEmailAsync(string mail, long chatId)
    {
        var user = await _userRepository.GetUserAsync(chatId);
        user.Email = mail;
        return await _userRepository.UpdateUserAsync(user);
    }
    public async Task<bool> UpdateActivityAsync(bool activity, long chatId)
    {
        var user = await _userRepository.GetUserAsync(chatId);
        user.IsActive = activity;
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeliverFileAsync(string fileName, string fileId, long chatId)
    {
        var user = await _userRepository.GetUserAsync(chatId);
        
        
        var data = new DownloadContext(fileName, chatId.ToString(), fileId);
        var path = await _fileDownloadService.SaveAsync(data);
        await _senderService.SendFileAsync(path, user.Email);
    }

    public async Task<bool> CheckUserIsActiveAsync(long chatId)
    {
        var user = await _userRepository.GetUserAsync(chatId);
        if (user != null) return user.IsActive;

        return false;
    }
}