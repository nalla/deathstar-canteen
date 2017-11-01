using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class AddCommand : Command
	{
		private Regex Regex { get; } = new Regex( @"(\d{2})\.?(\d{2})\.?(\d{4})\s(\w.*)", RegexOptions.Compiled );

		public AddCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override string Handle()
		{
			Match match = Regex.Match( Arguments ?? string.Empty );

			if( !match.Success )
				return "You need to provide some valid input.";

			string date = $"{match.Groups[3].Value}{match.Groups[2].Value}{match.Groups[1]}";
			string formattedDate = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";
			string meal = match.Groups[4].Value;
			Menu menu = MongoCollection.Find( x => x.Date == date ).SingleOrDefault();

			if( menu != null )
			{
				if( menu.Meals.Contains( meal ) )
					return $"_{meal}_ is already on the menu on *{formattedDate}*!";

				List<string> list = menu.Meals.ToList();

				list.Add( meal );
				menu.Meals = list.ToArray();
				MongoCollection.ReplaceOne( x => x.Id == menu.Id, menu );

				return $"I added _{meal}_ to the menu on *{formattedDate}*.";
			}

			menu = new Menu
			{
				Date = date,
				Meals = new[] { meal }
			};
			MongoCollection.InsertOne( menu );

			return $"I added _{meal}_ to the menu on *{formattedDate}*.";
		}
	}
}
