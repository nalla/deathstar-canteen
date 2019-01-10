using System;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;

namespace Deathstar.Canteen.Services
{
	public class DayAfterTomorrowCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly ISlackbot slackbot;

		public DayAfterTomorrowCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *dayaftertomorrow* command will return a list of the day after tomorrow's meals.";

		public string SupportedCommandName { get; } = "dayaftertomorrow";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			DateTime date = DateTime.Today.AddDays(2);
			Menu menu = await menuRepository.SingleOrDefaultAsync(x => x.Date == date.ToString("yyyyMMdd"), cancellationToken);
			string response = menu == null
				? "I don't know which meals are being served the day after tomorrow!"
				: $"The day after tomorrow is the *{date:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}";

			slackbot.SendMessage(channel, response);
		}
	}
}
