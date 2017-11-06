using System;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class DayAfterTomorrowCommand : DateCommand
	{
		public DayAfterTomorrowCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		protected override DateTime Date { get; } = DateTime.Today.AddDays( 2 );

		protected override string Description { get; } = "The day after tomorrow";
	}
}