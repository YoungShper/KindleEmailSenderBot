using KindleEmailSenderBot.Application.Files;
using KindleEmailSenderBot.Infrastructure;
using KindleEmailSenderBot.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace KindleEmailSenderBot.Web.Services;

public class SmtpFileSenderService : IFileSenderService
{
    private readonly SmtpClientFactory emailClientFactory;
    private readonly string email;
    private readonly ILogger<SmtpFileSenderService> logger;
    
    public SmtpFileSenderService(SmtpClientFactory emailClientFactory, ILogger<SmtpFileSenderService> logger, IOptions<SmtpOptions> smtpOptions)
    {
       this.emailClientFactory = emailClientFactory;
       email = smtpOptions.Value.Username;
       this.logger = logger;
    }

    public async Task SendFileAsync(string path, string to)
    {
        var smtpClient = await emailClientFactory.CreateAsync();
            
        var message = new MimeMessage();
        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Kindle Email Sender Bot";
        await bodyBuilder.Attachments.AddAsync(path);
        message.Body = bodyBuilder.ToMessageBody();

        message.Subject = "Kindle Email Sender Bot";
        message.From.Add(new MailboxAddress("Kindle Email Sender Bot", email));
        message.To.Add(new MailboxAddress("", to));
        var response = await smtpClient.SendAsync(message);
        logger.LogInformation($"Sending message to {to} is {response}");
        await smtpClient.DisconnectAsync(true);
    }
}