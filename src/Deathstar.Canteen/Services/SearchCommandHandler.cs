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
	public class SearchCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly Regex regex = new Regex(@"\w[\w\s]*", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public SearchCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *search* command will query future meals and displays the found menus."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `search Foobar`";

		public string SupportedCommandName { get; } = "search";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			if (regex.IsMatch(arguments ?? string.Empty))
			{
				FilterDefinition<Menu> filter = $"{{Meals: {{ $regex: '.*{arguments}.*' }}, Date: {{ $gte: '{DateTime.Today:yyyyMMdd}' }} }}";
				IEnumerable<Menu> menus = await menuRepository.ToListAsync(filter, cancellationToken) ?? new List<Menu>();

				if (menus.Any())
				{
					slackbot.SendMessage(channel, menus.Count() <= 10 ? GetMessage(menus).Trim() : "I found more than 10 menus. Please be more precise.");
				}
				else
				{
					slackbot.SendMessage(channel, "I found nothing.");
				}
			}
			else
			{
				slackbot.SendMessage(channel, "You need to provide some valid input.");
			}
		}

		private static string GetMessage(IEnumerable<Menu> menus)
		{
			var sb = new StringBuilder();

			foreach (Menu menu in menus)
			{
				DateTime date = DateTime.ParseExact(menu.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
				sb.Append($"On *{date:dd.MM.yyyy}* the meals are:{Environment.NewLine}{menu}{Environment.NewLine}{Environment.NewLine}");
			}

			return sb.ToString();
		}
	}
}
