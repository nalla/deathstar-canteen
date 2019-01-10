using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;

namespace Deathstar.Canteen.Services
{
	public interface IChatResponseRepository
	{
		Task AddAsync(string regex, string response);

		Task<IEnumerable<ChatResponse>> GetAsync(CancellationToken cancellationToken);

		Task<bool> RemoveAsync(string regex);
	}
}
