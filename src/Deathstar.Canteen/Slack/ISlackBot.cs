using System;
using System.Threading.Tasks;
using Slackbot;

namespace Deathstar.Canteen.Slack
{
	public interface ISlackbot
	{
		void AddMessageHandler(Func<OnMessageArgs, Task> handler);

		void SendMessage(string channel, string message);
	}
}
