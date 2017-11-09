using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public abstract class DateCommand : Command
	{
		protected DateCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		protected abstract DateTime Date { get; }

		protected abstract string Description { get; }

		public sealed override async Task<string> HandleAsync()
		{
			IAsyncCursor<Menu> cursor = await MongoCollection.FindAsync( x => x.Date == Date.ToString( "yyyyMMdd" ) );
			Menu menu = await cursor.SingleOrDefaultAsync();

			return menu == null
				? $"I don't know which meals are being served {Description.ToLower()}!"
				: $"{Description} is the *{Date:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}";
		}
	}
}
