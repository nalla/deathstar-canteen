using System.Threading;
using System.Threading.Tasks;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public interface ICommand
	{
		Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken);
	}
}
