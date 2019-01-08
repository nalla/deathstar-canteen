using System.Collections.Generic;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public interface ICommandFactory
	{
		ICommand GetCommand(string name);

		IEnumerable<ICommand> GetSupportedCommands();
	}
}
