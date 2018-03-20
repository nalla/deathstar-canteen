using System.Text.RegularExpressions;
using Deathstar.Canteen.Commands.Abstractions;
using Slackbot;

namespace Deathstar.Canteen.Commands
{
	public class CommandMessageParser : ICommandMessageParser
	{
		public CommandMessage Parse(OnMessageArgs message)
		{
			Match match = Regex.Match(message.Text, "(?:<@[A-Z0-9]+>\\s|)(\\w+)\\s?(.*)", RegexOptions.Compiled);
			string response = match.Groups.Count == 3 ? match.Groups[2].Value : string.Empty;

			return match.Success ? new CommandMessage(match.Groups[1].Value, message.Channel, response) : null;
		}
	}
}
