using System;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class DayAfterTomorrowCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly ISlackbot slackbot;

		public DayAfterTomorrowCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			DateTime date = DateTime.Today.AddDays(2);
			Menu menu = await menuCollection.SingleOrDefaultAsync(x => x.Date == date.ToString("yyyyMMdd"), cancellationToken);
			string response = menu == null
				? "I don't know which meals are being served the day after tomorrow!"
				: $"The day after tomorrow is the *{date:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}";

			slackbot.SendMessage(message.Channel, response);
		}
	}
}
