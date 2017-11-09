using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public abstract class Command : ICommand
	{
		protected Command( string arguments, IMongoClient mongoClient )
		{
			IMongoDatabase mongoDatabase = mongoClient.GetDatabase( "canteen" );
			MongoCollection = mongoDatabase.GetCollection<Menu>( "menus" );
			MongoCollection.Indexes.CreateOne( Builders<Menu>.IndexKeys.Ascending( x => x.Date ) );
			Arguments = arguments;
		}

		protected string Arguments { get; }

		protected bool HasArguments => !string.IsNullOrWhiteSpace( Arguments );

		protected IMongoCollection<Menu> MongoCollection { get; }

		public abstract Task<string> HandleAsync();

		protected void Log( string message ) => Console.WriteLine( $"{GetType().Name}: {message}" );
	}
}
