using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Slackbot;

namespace Deathstar.Canteen.Slack
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class Slackbot : Bot, ISlackbot
	{
		public Slackbot(IConfiguration configuration)
			: base(configuration["Slackbot:Token"], configuration["Slackbot:Username"])
		{
		}

		public void AddMessageHandler(Func<OnMessageArgs, Task> handler) => OnMessage += async (sender, message) => await handler(message);
	}
}
