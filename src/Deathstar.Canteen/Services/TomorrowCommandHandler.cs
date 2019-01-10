using System;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;

namespace Deathstar.Canteen.Services
{
	public class TomorrowCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly ISlackbot slackbot;

		public TomorrowCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *tomorrow* command will return a list of tomorrow's meals.";

		public string SupportedCommandName { get; } = "tomorrow";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			DateTime date = DateTime.Today.AddDays(1);
			Menu menu = await menuRepository.SingleOrDefaultAsync(x => x.Date == date.ToString("yyyyMMdd"), cancellationToken);
			string response = menu == null
				? "I don't know which meals are being served tomorrow!"
				: $"Tomorrow is the *{date:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}";

			slackbot.SendMessage(channel, response);
		}
	}
}
