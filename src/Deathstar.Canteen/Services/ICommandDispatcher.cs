using System.Threading;
using System.Threading.Tasks;
using Slackbot;

namespace Deathstar.Canteen.Services
{
	public interface ICommandDispatcher
	{
		Task DispatchAsync(OnMessageArgs message, CancellationToken cancellationToken);
	}
}
