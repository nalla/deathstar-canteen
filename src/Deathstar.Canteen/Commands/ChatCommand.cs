using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class ChatCommand : ICommand
	{
		private readonly IChatResponseCollection chatResponseCollection;
		private readonly Regex regex = new Regex(@"(add|remove)\s(\S+)\s(\S.*)", RegexOptions.Compiled);
		private readonly ISlackbot slackbot;

		public ChatCommand(IChatResponseCollection chatResponseCollection, ISlackbot slackbot)
		{
			this.chatResponseCollection = chatResponseCollection;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *chat* command can be use do add chat responses via regex."
			+ Environment.NewLine
			+ Environment.NewLine
			+ "Example: `chat add foo bar` or `chat remove foo`";

		public string Name { get; } = "chat";

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			Match match = regex.Match(message.Arguments);

			if (!match.Success)
			{
				foreach (ChatResponse chatResponse in await chatResponseCollection.GetAsync(cancellationToken))
				{
					if (new Regex(chatResponse.Regex).IsMatch(message.Arguments ?? string.Empty))
					{
						slackbot.SendMessage(message.Channel, chatResponse.Response);

						break;
					}
				}

				return;
			}

			switch (match.Groups[1].Value)
			{
				case "add":

					await chatResponseCollection.AddAsync(match.Groups[2].Value, match.Groups[3].Value);

					break;

				case "remove":
					await chatResponseCollection.RemoveAsync(match.Groups[2].Value);

					break;
			}
		}
	}
}
