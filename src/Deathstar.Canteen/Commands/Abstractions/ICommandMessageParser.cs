using Slackbot;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public interface ICommandMessageParser
	{
		CommandMessage Parse(OnMessageArgs message);
	}
}
