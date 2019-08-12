using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;

namespace Deathstar.Canteen.Services
{
	public class ChatCommandHandler : ICommandHandler
	{
		private readonly IChatResponseRepository chatResponseRepository;
		private readonly Regex regex = new Regex(@"^(add|remove)\s(\S+)\s?(\S?.*)", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public ChatCommandHandler(IChatResponseRepository chatResponseRepository, ISlackbot slackbot)
		{
			this.chatResponseRepository = chatResponseRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *chat* command can be use do add chat responses via regex."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `chat add foo bar` or `chat remove foo`";

		public string SupportedCommandName { get; } = "chat";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			Match match = regex.Match(arguments ?? string.Empty);

			if (match.Success)
			{
				switch (match.Groups[1].Value)
				{
					case "add":

						await chatResponseRepository.AddAsync(match.Groups[2].Value, match.Groups[3].Value);
						slackbot.SendMessage(channel, "I added your chat response to my AI.");

						break;

					case "remove":
						if (await chatResponseRepository.RemoveAsync(match.Groups[2].Value))
						{
							slackbot.SendMessage(channel, "I just forgot your response. Can't remember a thing.");
						}

						break;
				}
			}
			else
			{
				foreach (ChatResponse chatResponse in await chatResponseRepository.GetAsync(cancellationToken))
				{
					if (new Regex(chatResponse.Regex).IsMatch(arguments ?? string.Empty))
					{
						slackbot.SendMessage(channel, chatResponse.Response);

						break;
					}
				}
			}
		}
	}
}
