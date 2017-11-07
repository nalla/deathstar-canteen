using System;
using System.Collections.Generic;
using System.Text;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class HelpCommand : Command
	{
		private IEnumerable<string> SupportedCommands { get; } = new[]
		{
			"help",
			"today",
			"tomorrow",
			"dayaftertomorrow",
			"next",
			"add",
			"clear",
			"import",
			"stats"
		};

		public HelpCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override string Handle()
		{
			return HasArguments ? GetDetailedHelpMessage() : GetGeneralHelpMessage();
		}

		private string GetDetailedHelpMessage()
		{
			switch( Arguments.ToLower() )
			{
				case "help":
					return "The *help* command will return a list of supported commands.";

				case "today":
					return "The *today* command will return a list of today's meals.";

				case "tomorrow":
					return "The *tomorrow* command will return a list of tomorrow's meals.";

				case "dayaftertomorrow":
					return "The *dayaftertomorrow* command will return a list of the day after tomorrow's meals.";

				case "next":
					return "The *next* command will return a list of menus of the next days."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `next 5`";

				case "add":
					return "The *add* command can be used to add something to the menu."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `add 01012017 Foobar`";

				case "clear":
					return "The *clear* command can be used to clear the menu on a given date."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `clear 01012017`";

				case "import":
					return "The *import* command can be used to import a json based list of menus."
						+ Environment.NewLine
						+ Environment.NewLine
						+ "Example: `import https://some.url/endpoint`";

				case "stats":
					return "The *stats* command will display internal statistics of the canteen.";

				default:
					return GetGeneralHelpMessage();
			}
		}

		private string GetGeneralHelpMessage()
		{
			var sb = new StringBuilder();

			sb.AppendLine( "The following commands are available:" );
			foreach( string command in SupportedCommands )
				sb.AppendLine( $"  *{command}*" );

			sb.AppendLine();
			sb.Append( "Use *help command* for more information about each command." );

			return sb.ToString();
		}
	}
}
