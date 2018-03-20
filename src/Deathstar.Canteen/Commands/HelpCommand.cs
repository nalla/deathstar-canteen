using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class HelpCommand : ICommand
	{
		private readonly ISlackbot slackbot;

		private readonly IEnumerable<string> supportedCommands = new[]
		{
			"help",
			"today",
			"tomorrow",
			"dayaftertomorrow",
			"next",
			"search",
			"add",
			"clear",
			"import",
			"stats",
		};

		public HelpCommand(ISlackbot slackbot) => this.slackbot = slackbot;

		public Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			slackbot.SendMessage(message.Channel, string.IsNullOrWhiteSpace(message.Arguments) ? GetGeneralHelpMessage() : GetDetailedHelpMessage(message.Arguments));

			return Task.CompletedTask;
		}

		private string GetDetailedHelpMessage(string arguments)
		{
			string commandName = arguments.ToLower(CultureInfo.InvariantCulture);

			switch (commandName)
			{
				case "help":

					return "The *help* command will return a list of supported commands.";

				case "today":

					return "The *today* command will return a list of today's meals.";

				case "tomorrow":

					return "The *tomorrow* command will return a list of tomorrow's meals.";

				case "dayaftertomorrow":

					return "The *dayaftertomorrow* command will return a list of the day after tomorrow's meals.";

				case "next":

					return "The *next* command will return a list of menus of the next days."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `next 5`";

				case "search":

					return "The *search* command will query future meals and displays the found menus."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `search Foobar`";

				case "add":

					return "The *add* command can be used to add something to the menu."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `add 01012017 Foobar`";

				case "clear":

					return "The *clear* command can be used to clear the menu on a given date."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `clear 01012017`";

				case "import":

					return "The *import* command can be used to import a json based list of menus."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `import https://some.url/endpoint`";

				case "stats":

					return "The *stats* command will display internal statistics of the canteen.";

				default:

					return GetGeneralHelpMessage();
			}
		}

		private string GetGeneralHelpMessage()
		{
			var sb = new StringBuilder();

			sb.AppendLine("The following commands are available:");
			foreach (string command in supportedCommands)
			{
				sb.AppendLine($"  *{command}*");
			}

			sb.AppendLine();
			sb.Append("Use *help command* for more information about each command.");

			return sb.ToString();
		}
	}
}
