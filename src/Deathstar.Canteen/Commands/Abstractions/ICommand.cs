using System.Threading;
using System.Threading.Tasks;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public interface ICommand
	{
		string HelpText { get; }

		string Name { get; }

		Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken);
	}
}
