using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class NextCommand : Command
	{
		private Regex Regex { get; } = new Regex( @"(\d+)", RegexOptions.Compiled );

		public NextCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override string Handle()
		{
			Match match = Regex.Match( Arguments ?? string.Empty );

			if( !match.Success )
				return "You need to provide some valid input.";

			int days = Math.Min( Math.Max( int.Parse( match.Groups[1].Value ), 1 ), 7 );
			List<Menu> menus = MongoCollection.Find( $"{{Date: {{ $gte: '{DateTime.Today:yyyyMMdd}', $lt: '{DateTime.Today.AddDays( days ):yyyyMMdd}' }} }}" ).ToList();

			if( menus == null || menus.Count == 0 )
				return $"I don't know which meals are being served the next {days} days!";

			string response = string.Empty;

			foreach( Menu menu in menus )
			{
				var date = DateTime.ParseExact( menu.Date, "yyyyMMdd", CultureInfo.InvariantCulture );
				response += $"On *{date:dd.MM.yyyy}* the meals are:{Environment.NewLine}{menu}{Environment.NewLine}{Environment.NewLine}";
			}

			return response.Trim();
		}
	}
}
