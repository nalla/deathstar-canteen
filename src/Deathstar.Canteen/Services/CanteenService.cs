using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Slack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot;

namespace Deathstar.Canteen.Services
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class CanteenService : BackgroundService
	{
		private readonly ICommandFactory commandFactory;

		private readonly ICommandMessageParser commandMessageParser;

		private readonly IConfiguration configuration;

		private readonly ILogger logger;

		private readonly ISlackbot slackbot;

		public CanteenService(
			IConfiguration configuration,
			ICommandMessageParser commandMessageParser,
			ICommandFactory commandFactory,
			ISlackbot slackbot,
			ILogger<CanteenService> logger)
		{
			this.configuration = configuration;
			this.commandMessageParser = commandMessageParser;
			this.commandFactory = commandFactory;
			this.slackbot = slackbot;
			this.logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			slackbot.AddMessageHandler(async message =>
			{
				try
				{
					if (message.MentionedUsers.Any(user => user == configuration["Slackbot:Username"]))
					{
						await Handle(message, stoppingToken);
					}
				}
				catch (Exception e)
				{
					logger.LogError(e, e.Message);
				}
			});

			await Task.Delay(-1, stoppingToken);
		}

		private async Task Handle(OnMessageArgs message, CancellationToken stoppingToken)
		{
			ICommandMessage commandMessage = commandMessageParser.Parse(message);
			ICommand command = commandFactory.GetCommand(commandMessage?.Name);

			if (command == null)
			{
				commandMessage = new CommandMessage("chat", message.Channel, message.Text);
				command = commandFactory.GetCommand(commandMessage.Name);
			}

			if (command != null)
			{
				await command.HandleAsync(commandMessage, stoppingToken);
			}
		}
	}
}
