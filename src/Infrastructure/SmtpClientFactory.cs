using KindleEmailSenderBot.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KindleEmailSenderBot.Infrastructure;

// TODO: это должен быть синглтон, какая фабрика клиентов? Шиза!
public class SmtpClientFactory
{
    private ILogger<SmtpClientFactory> logger;
    private readonly SmtpOptions smtpOptions;
    public SmtpClientFactory(IOptions<SmtpOptions> smtpSettings, ILogger<SmtpClientFactory> logger)
    {
        smtpOptions = smtpSettings.Value;
        this.logger = logger;
    }
    
    public async Task<ISmtpClient> CreateAsync()
    {
        var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(smtpOptions.Username, smtpOptions.Password);
        logger.LogInformation(message:"Connected to smtp server");
        return client;
    }
}