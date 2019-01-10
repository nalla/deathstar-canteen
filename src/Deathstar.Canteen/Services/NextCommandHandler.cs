using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Services
{
	public class NextCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly Regex regex = new Regex(@"(\d+)", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public NextCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *next* command will return a list of menus of the next days."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `next 5`";

		public string SupportedCommandName { get; } = "next";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			Match match = regex.Match(arguments ?? string.Empty);

			if (match.Success)
			{
				int days = Math.Min(Math.Max(int.Parse(match.Groups[1].Value), 1), 7);
				FilterDefinition<Menu> filter = $"{{Date: {{ $gte: '{DateTime.Today:yyyyMMdd}', $lt: '{DateTime.Today.AddDays(days):yyyyMMdd}' }} }}";
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
					slackbot.SendMessage(channel, $"I don't know which meals are being served the next {days} days!");
				}
			}
			else
			{
				slackbot.SendMessage(channel, "You need to provide some valid input.");
			}
		}
	}
}
