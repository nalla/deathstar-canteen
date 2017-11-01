using System;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class TomorrowCommand : DateCommand
	{
		public TomorrowCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		protected override DateTime Date { get; } = DateTime.Today.AddDays( 1 );

		protected override string Description { get; } = "Tomorrow";
	}
}
