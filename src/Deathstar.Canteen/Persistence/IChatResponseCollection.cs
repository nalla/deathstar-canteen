using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Deathstar.Canteen.Persistence
{
	public interface IChatResponseCollection
	{
		Task AddAsync(string regex, string response);

		Task<IEnumerable<ChatResponse>> GetAsync(CancellationToken cancellationToken);

		Task<bool> RemoveAsync(string regex);
	}
}
