using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Slack;
using Microsoft.Extensions.Configuration;

namespace Deathstar.Canteen.Commands
{
	public class ChatCommand : ICommand
	{
		private readonly IConfiguration configuration;
		private readonly ISlackbot slackbot;

		public ChatCommand(IConfiguration configuration, ISlackbot slackbot)
		{
			this.configuration = configuration;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The default command that tries to chat.";

		public string Name { get; } = "chat";

		public Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			foreach (ChatResponse chatResponse in configuration.GetSection("chat").Get<ChatResponse[]>())
			{
				if (new Regex(chatResponse.Regex).IsMatch(message.Arguments ?? string.Empty))
				{
					slackbot.SendMessage(message.Channel, chatResponse.Response);

					break;
				}
			}

			return Task.CompletedTask;
		}
	}
}
