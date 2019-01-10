using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Deathstar.Canteen.Services
{
	public class ClearCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly Regex regex = new Regex(@"(\d{2})\.?(\d{2})\.?(\d{4})", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public ClearCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *clear* command can be used to clear the menu on a given date."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `clear 01012017`";

		public string SupportedCommandName { get; } = "clear";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			Match match = regex.Match(arguments ?? string.Empty);

			if (!match.Success)
			{
				slackbot.SendMessage(channel, "You need to provide some valid input.");
			}

			{
				string date = $"{match.Groups[3].Value}{match.Groups[2].Value}{match.Groups[1]}";
				string formattedDate = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";
				string response = $"There is no menu on *{formattedDate}*!";

				if (await menuRepository.DeleteOneAsync(x => x.Date == date, cancellationToken) == 1)
				{
					response = $"I cleared the menu on *{formattedDate}*.";
				}

				slackbot.SendMessage(channel, response);
			}
		}
	}
}
