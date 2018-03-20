using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace Deathstar.Canteen.Commands
{
	public class ImportCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly Regex regex = new Regex("^\\d{8}$", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;
		private readonly ILogger logger;

		public ImportCommand(IMenuCollection menuCollection, ISlackbot slackbot, ILogger<ImportCommand> logger)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
			this.logger = logger;
		}

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			var url = new Url((message.Arguments ?? string.Empty).TrimStart('<').TrimEnd('>'));

			if (!url.IsValid())
			{
				slackbot.SendMessage(message.Channel, "You need to provide a well formed url.");

				return;
			}

			try
			{
				Menu[] menus = await url.GetJsonAsync<Menu[]>(cancellationToken);

				var i = 0;

				foreach (Menu menu in menus)
				{
					// ReSharper disable once InvertIf
					if (regex.IsMatch(menu.Date ?? string.Empty) &&
						menu.Meals?.Length > 0 &&
						menu.Meals.All(x => !string.IsNullOrWhiteSpace(x)) &&
						await menuCollection.CountAsync(x => x.Date == menu.Date, cancellationToken) == 0)
					{
						await menuCollection.InsertOneAsync(menu, cancellationToken);
						i++;
					}
				}

				slackbot.SendMessage(message.Channel, $"I imported {i} menus.");
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);
				slackbot.SendMessage(message.Channel, "I got an error while downloading the url you provided.");
			}
		}
	}
}
