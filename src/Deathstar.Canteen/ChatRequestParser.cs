using System.Linq;
using System.Text.RegularExpressions;
using Slackbot;

namespace Deathstar.Canteen
{
	public class ChatRequestParser
	{
		private Regex Regex { get; }

		private string Username { get; }

		public ChatRequestParser( string username, string regex )
		{
			Username = username;
			Regex = new Regex( regex );
		}

		public bool Parse( OnMessageArgs message )
		{
			return message.MentionedUsers.Any( user => user == Username ) && Regex.IsMatch( message.Text );
		}
	}
}
