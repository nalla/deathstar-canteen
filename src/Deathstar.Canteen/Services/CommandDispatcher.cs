using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Slackbot;

namespace Deathstar.Canteen.Services
{
	public class CommandDispatcher : ICommandDispatcher
	{
		private readonly IEnumerable<ICommandHandler> commandHandlers;

		public CommandDispatcher(IEnumerable<ICommandHandler> commandHandlers) => this.commandHandlers = commandHandlers;

		public Task DispatchAsync(OnMessageArgs message, CancellationToken cancellationToken)
		{
			Match match = Regex.Match(message.Text, "(?:<@[A-Z0-9]+>\\s|)(\\w+)\\s?(.*)", RegexOptions.Compiled);

			if (match.Success)
			{
				string name = match.Groups[1].Value;
				string arguments = match.Groups.Count == 3 ? match.Groups[2].Value : string.Empty;
				ICommandHandler commandHandler = commandHandlers.FirstOrDefault(x => x.SupportedCommandName == name);

				if (commandHandler != null)
				{
					return commandHandler.HandleAsync(arguments, message.Channel, cancellationToken);
				}
			}

			return commandHandlers.First(x => x.SupportedCommandName == "chat").HandleAsync(message.Text, message.Channel, cancellationToken);
		}
	}
}
