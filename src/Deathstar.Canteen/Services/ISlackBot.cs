using System;
using System.Threading.Tasks;
using Slackbot;

namespace Deathstar.Canteen.Services
{
	public interface ISlackbot
	{
		void AddMessageHandler(Func<OnMessageArgs, Task> handler);

		void SendMessage(string channel, string message);
	}
}
