using System.Threading;
using System.Threading.Tasks;

namespace Deathstar.Canteen.Services
{
	public interface ICommandHandler
	{
		string HelpText { get; }

		string SupportedCommandName { get; }

		Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken);
	}
}
