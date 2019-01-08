using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class AddCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly Regex regex = new Regex(@"(\d{2})\.?(\d{2})\.?(\d{4})\s(\w.*)", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public AddCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *add* command can be used to add something to the menu."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `add 01012017 Foobar`";

		public string Name { get; } = "add";

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			Match match = regex.Match(message.Arguments ?? string.Empty);

			if (!match.Success)
			{
				slackbot.SendMessage(message.Channel, "You need to provide some valid input.");
			}

			string date = $"{match.Groups[3].Value}{match.Groups[2].Value}{match.Groups[1]}";
			string formattedDate = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";
			string meal = match.Groups[4].Value;
			Menu menu = await menuCollection.SingleOrDefaultAsync(x => x.Date == date, cancellationToken);

			if (menu != null)
			{
				if (menu.Meals.Contains(meal))
				{
					slackbot.SendMessage(message.Channel, $"_{meal}_ is already on the menu on *{formattedDate}*!");
				}
				else
				{
					List<string> list = menu.Meals.ToList();

					list.Add(meal);
					menu.Meals = list.ToArray();
					await menuCollection.ReplaceOneAsync(x => x.Id == menu.Id, menu, cancellationToken);
					slackbot.SendMessage(message.Channel, $"I added _{meal}_ to the menu on *{formattedDate}*.");
				}
			}
			else
			{
				menu = new Menu
				{
					Date = date,
					Meals = new[] { meal },
				};
				await menuCollection.InsertOneAsync(menu, cancellationToken);
				slackbot.SendMessage(message.Channel, $"I added _{meal}_ to the menu on *{formattedDate}*.");
			}
		}
	}
}
