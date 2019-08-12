using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Services
{
	public class WeekCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly ISlackbot slackbot;

		public WeekCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *week* command will return a list of menus of the current week.";

		public string SupportedCommandName { get; } = "week";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			int diff = CalculateDifference();
			DateTime startOfNextWeek = DateTime.Today.AddDays(diff).Date;
			FilterDefinition<Menu> filter = $"{{Date: {{ $gte: '{DateTime.Today:yyyyMMdd}', $lt: '{startOfNextWeek:yyyyMMdd}' }} }}";
			IEnumerable<Menu> menus = await menuRepository.ToListAsync(filter, cancellationToken) ?? new List<Menu>();

			if (menus.Any())
			{
				var sb = new StringBuilder();

				foreach (Menu menu in menus)
				{
					DateTime date = DateTime.ParseExact(menu.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
					sb.Append($"On *{date:dd.MM.yyyy}* the meals are:{Environment.NewLine}{menu}{Environment.NewLine}{Environment.NewLine}");
				}

				slackbot.SendMessage(channel, sb.ToString().Trim());
			}
			else
			{
				slackbot.SendMessage(channel, "I don't know which meals are being served this week!");
			}
		}

		private static int CalculateDifference()
		{
			switch (DateTime.Today.DayOfWeek)
			{
				case DayOfWeek.Monday:
					return 7;
				case DayOfWeek.Tuesday:
					return 6;
				case DayOfWeek.Wednesday:
					return 5;
				case DayOfWeek.Thursday:
					return 4;
				case DayOfWeek.Friday:
					return 3;
				case DayOfWeek.Saturday:
					return 2;
				case DayOfWeek.Sunday:
					return 1;
				default:
					return 0;
			}
		}
	}
}
