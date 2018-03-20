using Deathstar.Canteen.Commands.Abstractions;

namespace Deathstar.Canteen.Tests.Mocks
{
	internal class FakeCommandMessage : ICommandMessage
	{
		public FakeCommandMessage(string arguments, string channel)
		{
			Arguments = arguments;
			Channel = channel;
			Name = string.Empty;
		}

		public string Arguments { get; }

		public string Channel { get; }

		public string Name { get; }
	}
}
