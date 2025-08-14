using KindleEmailSenderBot.Core.Interfaces;
using KindleEmailSenderBot.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KindleEmailSenderBot.Infrastructure;

public class SmtpClientFactory
{
    private ILogger<SmtpClientFactory> _logger;
    private readonly SmtpOptions _smtpOptions;
    public SmtpClientFactory(IOptions<SmtpOptions> smtpSettings, ILogger<SmtpClientFactory> logger)
    {
        _smtpOptions = smtpSettings.Value;
        _logger = logger;
    }
    
    public async Task<ISmtpClient> CreateAsync()
    {
        var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        await client.ConnectAsync(_smtpOptions.Server, _smtpOptions.Port, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password);
        _logger.LogInformation(message:"Connected to smtp server");
        return client;
    }
}