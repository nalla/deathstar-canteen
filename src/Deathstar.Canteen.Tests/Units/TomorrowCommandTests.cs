using System;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class TomorrowCommandTests
	{
		public TomorrowCommandTests() => MongoHelper.Clear();

		[Fact]
		public void TheHandleMethodShouldReturnErrorNoticeWhenNoMenuisPresent()
		{
			// Arrange
			var command = new TomorrowCommand( null, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "I don't know which meals are being served tomorrow!", response );
		}

		[Fact]
		public void TheHandleMethodShouldReturnTomorrowsMenu()
		{
			// Arrange
			var menu = new Menu { Date = DateTime.Today.AddDays( 1 ).ToString( "yyyyMMdd" ), Meals = new[] { "Foo", "Bar" } };
			MongoHelper.Collection.InsertOne( menu );
			var command = new TomorrowCommand( null, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"Tomorrow is the *{DateTime.Today.AddDays( 1 ):dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}", response );
		}
	}
}
