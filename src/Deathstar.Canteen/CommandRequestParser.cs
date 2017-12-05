using System.Linq;
using System.Text.RegularExpressions;
using Slackbot;

namespace Deathstar.Canteen
{
	public class CommandRequestParser
	{
		private Regex Regex { get; } = new Regex( "(?:<@[A-Z0-9]+>\\s|)(\\w+)\\s?(.*)", RegexOptions.Compiled );

		private string Username { get; }

		public CommandRequestParser( string username )
		{
			Username = username;
		}

		public CommandRequest Parse( OnMessageArgs message )
		{
			if( message.MentionedUsers.All( user => user != Username ) )
				return null;

			Match match = Regex.Match( message.Text );

			return !match.Success ? null : new CommandRequest( match.Groups[1].Value, match.Groups.Count == 3 ? match.Groups[2].Value : string.Empty );
		}
	}
}
