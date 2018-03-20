using Deathstar.Canteen.Commands.Abstractions;

namespace Deathstar.Canteen.Commands
{
	public class CommandMessage : ICommandMessage
	{
		public CommandMessage(string name, string channel, string arguments = "")
		{
			Name = name;
			Channel = channel;
			Arguments = arguments;
		}

		public string Arguments { get; }

		public string Channel { get; }

		public string Name { get; }
	}
}
