using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Services;

namespace Deathstar.Canteen.Tests.Mocks
{
	internal class FakeCommandHandler : ICommandHandler
	{
		public FakeCommandHandler(string supportedCommandName) => SupportedCommandName = supportedCommandName;

		public string HelpText { get; } = "A fake command";

		public string ReceivedArguments { get; private set; }

		public string SupportedCommandName { get; }

		public Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			ReceivedArguments = arguments;

			return Task.CompletedTask;
		}
	}
}
