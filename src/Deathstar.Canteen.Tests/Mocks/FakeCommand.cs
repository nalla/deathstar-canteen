using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;

namespace Deathstar.Canteen.Tests.Mocks
{
	internal class FakeCommand : ICommand
	{
		public string HelpText { get; } = "A fake command";

		public string Name { get; } = "fake";

		public Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
