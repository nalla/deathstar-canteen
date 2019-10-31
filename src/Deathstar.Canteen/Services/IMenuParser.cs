using System.Collections.Generic;
using Deathstar.Canteen.Persistence;

namespace Deathstar.Canteen.Services
{
	public interface IMenuParser
	{
		IEnumerable<Menu> ParseText(string text);
	}
}
