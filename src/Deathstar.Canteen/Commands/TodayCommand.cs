using System;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class TodayCommand : DateCommand
	{
		public TodayCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		protected override DateTime Date { get; } = DateTime.Today;

		protected override string Description { get; } = "Today";
	}
}
