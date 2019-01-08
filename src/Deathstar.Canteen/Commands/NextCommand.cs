using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class NextCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly Regex regex = new Regex(@"(\d+)", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public NextCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *next* command will return a list of menus of the next days."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `next 5`";

		public string Name { get; } = "next";

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			Match match = regex.Match(message.Arguments ?? string.Empty);

			if (match.Success)
			{
				int days = Math.Min(Math.Max(int.Parse(match.Groups[1].Value), 1), 7);
				FilterDefinition<Menu> filter = $"{{Date: {{ $gte: '{DateTime.Today:yyyyMMdd}', $lt: '{DateTime.Today.AddDays(days):yyyyMMdd}' }} }}";
				IEnumerable<Menu> menus = await menuCollection.ToListAsync(filter, cancellationToken) ?? new List<Menu>();

				if (menus.Any())
				{
					var sb = new StringBuilder();

					foreach (Menu menu in menus)
					{
						DateTime date = DateTime.ParseExact(menu.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
						sb.Append($"On *{date:dd.MM.yyyy}* the meals are:{Environment.NewLine}{menu}{Environment.NewLine}{Environment.NewLine}");
					}

					slackbot.SendMessage(message.Channel, sb.ToString().Trim());
				}
				else
				{
					slackbot.SendMessage(message.Channel, $"I don't know which meals are being served the next {days} days!");
				}
			}
			else
			{
				slackbot.SendMessage(message.Channel, "You need to provide some valid input.");
			}
		}
	}
}
