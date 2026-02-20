using System.Xml.Linq;
using KindleEmailSenderBot.Web.Commands.Data;
using Microsoft.Extensions.Options;
using BotCommand = Telegram.Bot.Types.BotCommand;

namespace KindleEmailSenderBot.Web.Commands;

public class KindleBotCommandProvider
{
    private readonly CommandsOptions commandsOptions;
    private readonly IEnumerable<BotCommandData> commands;

    public KindleBotCommandProvider(IOptions<CommandsOptions> commandsOptions)
    {
        this.commandsOptions = commandsOptions.Value;
        commands = XDocument.Load(this.commandsOptions.Commands).Root?
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
        return commands;
    }
    
    public string? GetCommandResponseAsync(string input)
    {
        return GetCommands()
            .FirstOrDefault(x => x.BotCommand.Command == input)?
            .Response;
    }
}