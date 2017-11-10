using System.Threading.Tasks;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public interface ICommand
	{
		Task<string> HandleAsync();
	}
}
