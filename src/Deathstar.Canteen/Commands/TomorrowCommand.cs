using System;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class TomorrowCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly ISlackbot slackbot;

		public TomorrowCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *tomorrow* command will return a list of tomorrow's meals.";

		public string Name { get; } = "tomorrow";

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			DateTime date = DateTime.Today.AddDays(1);
			Menu menu = await menuCollection.SingleOrDefaultAsync(x => x.Date == date.ToString("yyyyMMdd"), cancellationToken);
			string response = menu == null
				? "I don't know which meals are being served tomorrow!"
				: $"Tomorrow is the *{date:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}";

			slackbot.SendMessage(message.Channel, response);
		}
	}
}
