using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class ClearCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly Regex regex = new Regex(@"(\d{2})\.?(\d{2})\.?(\d{4})", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public ClearCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			Match match = regex.Match(message.Arguments ?? string.Empty);

			if (!match.Success)
			{
				slackbot.SendMessage(message.Channel, "You need to provide some valid input.");
			}

			{
				string date = $"{match.Groups[3].Value}{match.Groups[2].Value}{match.Groups[1]}";
				string formattedDate = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";
				string response = $"There is no menu on *{formattedDate}*!";

				if (await menuCollection.DeleteOneAsync(x => x.Date == date, cancellationToken) == 1)
				{
					response = $"I cleared the menu on *{formattedDate}*.";
				}

				slackbot.SendMessage(message.Channel, response);
			}
		}
	}
}
