using System;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands.Abstractions
{
	public abstract class DateCommand : Command
	{
		protected DateCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		protected abstract DateTime Date { get; }

		protected abstract string Description { get; }

		public sealed override string Handle()
		{
			Menu menu = MongoCollection.Find( x => x.Date == Date.ToString( "yyyyMMdd" ) ).SingleOrDefault();

			return menu == null
				? $"I don't know which meals are being served {Description.ToLower()}!"
				: $"{Description} is the *{Date:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}";
		}
	}
}
