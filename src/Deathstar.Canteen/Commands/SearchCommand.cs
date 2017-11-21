using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class SearchCommand : Command
	{
		private Regex Regex { get; } = new Regex( @"\w[\w\s]*", RegexOptions.Compiled );

		public SearchCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override async Task<string> HandleAsync()
		{
			if( !Regex.IsMatch( Arguments ?? string.Empty ) )
				return "You need to provide some valid input.";

			IAsyncCursor<Menu> cursor = await MongoCollection.FindAsync( $"{{Meals: {{ $regex: '.*{Arguments}.*' }}, Date: {{ $gte: '{DateTime.Today:yyyyMMdd}' }} }}" );
			List<Menu> menus = await cursor.ToListAsync();

			if( menus == null || menus.Count == 0 )
				return "I found nothing.";

			if( menus.Count > 10 )
				return "I found more than 10 menus. Please be more precise.";

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
