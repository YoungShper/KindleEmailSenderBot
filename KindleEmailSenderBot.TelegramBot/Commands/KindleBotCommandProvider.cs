using System.Xml.Linq;
using KindleEmailSenderBot.TelegramBot.Commands.Data;
using Microsoft.Extensions.Options;
using BotCommand = Telegram.Bot.Types.BotCommand;

namespace KindleEmailSenderBot.TelegramBot.Commands;

public class KindleBotCommandProvider
{
    private readonly CommandsOptions _commandsOptions;
    private readonly IEnumerable<BotCommandData> Commands;

    public KindleBotCommandProvider(IOptions<CommandsOptions> commandsOptions)
    {
        _commandsOptions = commandsOptions.Value;
        Commands = XDocument.Load(_commandsOptions.Commands).Root?
            .Elements("Command")
            .Select(x => new BotCommandData
                {
                    BotCommand = new BotCommand{ Command = x.Attribute("name").Value, Description = x.Element("Description").Value },
                    CommandName = x.Attribute("name").Value,
                    Response = x.Element("Response").Value
                }
            );
    }
    
    public IEnumerable<BotCommandData> GetCommands()
    {
        return Commands;
    }
    
    public async Task<string> GetCommandResponseAsync(string input)
    {
        return await Task.FromResult(GetCommands().FirstOrDefault(x => x.BotCommand.Command == input).Response);
    }
}