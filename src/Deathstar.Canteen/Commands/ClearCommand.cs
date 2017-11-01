using System.Text.RegularExpressions;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class ClearCommand : Command
	{
		private Regex Regex { get; } = new Regex( @"(\d{2})\.?(\d{2})\.?(\d{4})", RegexOptions.Compiled );

		public ClearCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override string Handle()
		{
			Match match = Regex.Match( Arguments ?? string.Empty );

			if( !match.Success )
				return "You need to provide some valid input.";

			string date = $"{match.Groups[3].Value}{match.Groups[2].Value}{match.Groups[1]}";
			string formattedDate = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";

			return MongoCollection.DeleteOne( x => x.Date == date ).DeletedCount == 1
				? $"I cleared the menu on *{formattedDate}*."
				: $"There is no menu on *{formattedDate}*!";
		}
	}
}
