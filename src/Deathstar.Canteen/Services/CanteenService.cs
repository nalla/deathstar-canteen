using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot;

namespace Deathstar.Canteen.Services
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class CanteenService : BackgroundService
	{
		private readonly ICommandDispatcher commandDispatcher;

		private readonly IConfiguration configuration;

		private readonly ILogger logger;

		private readonly ISlackbot slackbot;

		public CanteenService(
			IConfiguration configuration,
			ICommandDispatcher commandDispatcher,
			ISlackbot slackbot,
			ILogger<CanteenService> logger)
		{
			this.configuration = configuration;
			this.commandDispatcher = commandDispatcher;
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
						await HandleAsync(message, stoppingToken);
					}
				}
				catch (Exception e)
				{
					logger.LogError(e, e.Message);
				}
			});

			await Task.Delay(-1, stoppingToken);
		}

		private Task HandleAsync(OnMessageArgs message, CancellationToken stoppingToken) => commandDispatcher.DispatchAsync(message, stoppingToken);
	}
}
