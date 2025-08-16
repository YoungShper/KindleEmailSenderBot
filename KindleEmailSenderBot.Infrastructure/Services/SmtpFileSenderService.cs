using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Infrastructure.Options;
using MimeKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KindleEmailSenderBot.Infrastructure;

public class SmtpFileSenderService : IFileSenderService
{
    ILogger<SmtpFileSenderService> _logger;
    SmtpClientFactory _emailClientFactory;
    private string _email;
    
    public SmtpFileSenderService(SmtpClientFactory emailClientFactory, ILogger<SmtpFileSenderService> logger, IOptions<SmtpOptions> smtpOptions)
    {
       _logger = logger;
       _emailClientFactory = emailClientFactory;
       _email = smtpOptions.Value.Username;
    }

    public async Task SendFileAsync(string path, string to)
    {
        var smtpClient = await _emailClientFactory.CreateAsync();
            
        var message = new MimeMessage();
        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Kindle Email Sender Bot";
        await bodyBuilder.Attachments.AddAsync(path);
        message.Body = bodyBuilder.ToMessageBody();

        message.Subject = "Kindle Email Sender Bot";
        message.From.Add(new MailboxAddress("Kindle Email Sender Bot", _email));
        message.To.Add(new MailboxAddress("", to));
        var response = await smtpClient.SendAsync(message);
        _logger.LogInformation($"Sending message to {to} is {response}");
        await smtpClient.DisconnectAsync(true);
    }
}