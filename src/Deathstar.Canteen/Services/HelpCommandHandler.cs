using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Deathstar.Canteen.Services
{
	public class HelpCommandHandler : ICommandHandler
	{
		private readonly IServiceProvider serviceProvider;
		private readonly ISlackbot slackbot;

		public HelpCommandHandler(ISlackbot slackbot, IServiceProvider serviceProvider)
		{
			this.slackbot = slackbot;
			this.serviceProvider = serviceProvider;
		}

		public string HelpText { get; } = "The *help* command will return a list of supported commands.";

		public string SupportedCommandName { get; } = "help";

		public Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			slackbot.SendMessage(channel, string.IsNullOrWhiteSpace(arguments) ? GetGeneralHelpMessage() : GetDetailedHelpMessage(arguments));

			return Task.CompletedTask;
		}

		private string GetDetailedHelpMessage(string arguments)
		{
			ICommandHandler commandHandler = serviceProvider.GetService<IEnumerable<ICommandHandler>>().FirstOrDefault(x => x.SupportedCommandName == arguments.ToLower(CultureInfo.InvariantCulture));

			if (commandHandler != null)
			{
				return commandHandler.HelpText;
			}

			return GetGeneralHelpMessage();
		}

		private string GetGeneralHelpMessage()
		{
			var sb = new StringBuilder();

			sb.AppendLine("The following commands are available:");

			foreach (string command in serviceProvider.GetService<IEnumerable<ICommandHandler>>().Select(x => x.SupportedCommandName))
			{
				sb.AppendLine($"  *{command}*");
			}

			sb.AppendLine();
			sb.Append("Use *help command* for more information about each command.");

			return sb.ToString();
		}
	}
}
