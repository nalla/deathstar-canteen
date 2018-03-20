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
	public class SearchCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly Regex regex = new Regex(@"\w[\w\s]*", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public SearchCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			if (!regex.IsMatch(message.Arguments ?? string.Empty))
			{
				slackbot.SendMessage(message.Channel, "You need to provide some valid input.");

				return;
			}

			FilterDefinition<Menu> filter = $"{{Meals: {{ $regex: '.*{message.Arguments}.*' }}, Date: {{ $gte: '{DateTime.Today:yyyyMMdd}' }} }}";
			IEnumerable<Menu> menus = await menuCollection.ToListAsync(filter, cancellationToken) ?? new List<Menu>();

			if (menus.Any())
			{
				if (menus.Count() <= 10)
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
					slackbot.SendMessage(message.Channel, "I found more than 10 menus. Please be more precise.");
				}
			}
			else
			{
				slackbot.SendMessage(message.Channel, "I found nothing.");
			}
		}
	}
}
