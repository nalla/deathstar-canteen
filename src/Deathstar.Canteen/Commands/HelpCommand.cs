using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Slack;
using Microsoft.Extensions.DependencyInjection;

namespace Deathstar.Canteen.Commands
{
	public class HelpCommand : ICommand
	{
		private readonly IServiceProvider serviceProvider;
		private readonly ISlackbot slackbot;

		public HelpCommand(ISlackbot slackbot, IServiceProvider serviceProvider)
		{
			this.slackbot = slackbot;
			this.serviceProvider = serviceProvider;
		}

		public string HelpText { get; } = "The *help* command will return a list of supported commands.";

		public string Name { get; } = "help";

		public Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			slackbot.SendMessage(message.Channel, string.IsNullOrWhiteSpace(message.Arguments) ? GetGeneralHelpMessage() : GetDetailedHelpMessage(message.Arguments));

			return Task.CompletedTask;
		}

		private string GetDetailedHelpMessage(string arguments)
		{
			ICommand command = serviceProvider.GetService<ICommandFactory>().GetCommand(arguments.ToLower(CultureInfo.InvariantCulture));

			if (command != null)
			{
				return command.HelpText;
			}

			return GetGeneralHelpMessage();
		}

		private string GetGeneralHelpMessage()
		{
			var sb = new StringBuilder();

			sb.AppendLine("The following commands are available:");

			foreach (string command in serviceProvider.GetService<ICommandFactory>().GetSupportedCommands().Select(x => x.Name))
			{
				sb.AppendLine($"  *{command}*");
			}

			sb.AppendLine();
			sb.Append("Use *help command* for more information about each command.");

			return sb.ToString();
		}
	}
}
