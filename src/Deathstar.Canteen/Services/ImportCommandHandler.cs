using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace Deathstar.Canteen.Services
{
	public class ImportCommandHandler : ICommandHandler
	{
		private readonly ILogger logger;
		private readonly IMenuRepository menuRepository;
		private readonly Regex regex = new Regex("^\\d{8}$", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public ImportCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot, ILogger<ImportCommandHandler> logger)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
			this.logger = logger;
		}

		public string HelpText { get; } = "The *import* command can be used to import a json based list of menus."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `import https://some.url/endpoint`";

		public string SupportedCommandName { get; } = "import";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			var url = new Url((arguments ?? string.Empty).TrimStart('<').TrimEnd('>'));

			if (!url.IsValid())
			{
				slackbot.SendMessage(channel, "You need to provide a well formed url.");

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
						await menuRepository.CountAsync(x => x.Date == menu.Date, cancellationToken) == 0)
					{
						await menuRepository.InsertOneAsync(menu, cancellationToken);
						i++;
					}
				}

				slackbot.SendMessage(channel, $"I imported {i} menus.");
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);
				slackbot.SendMessage(channel, "I got an error while downloading the url you provided.");
			}
		}
	}
}
