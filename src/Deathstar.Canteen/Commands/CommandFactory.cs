using System.Collections.Generic;
using System.Linq;
using Deathstar.Canteen.Commands.Abstractions;

namespace Deathstar.Canteen.Commands
{
	public class CommandFactory : ICommandFactory
	{
		private readonly IEnumerable<ICommand> commands;

		public CommandFactory(IEnumerable<ICommand> commands) => this.commands = commands;

		public ICommand GetCommand(string name) => commands?.FirstOrDefault(x => x.GetType().Name.ToLowerInvariant().StartsWith(name?.ToLowerInvariant() ?? string.Empty));
	}
}
