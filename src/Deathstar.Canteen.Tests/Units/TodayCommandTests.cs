using System;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class TodayCommandTests
	{
		public TodayCommandTests() => MongoHelper.Clear();

		[Fact]
		public void TheHandleMethodShouldReturnErrorNoticeWhenNoMenuisPresent()
		{
			// Arrange
			var command = new TodayCommand( null, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "I don't know which meals are being served today!", response );
		}

		[Fact]
		public void TheHandleMethodShouldReturnTodaysMenu()
		{
			// Arrange
			var menu = new Menu { Date = DateTime.Today.ToString( "yyyyMMdd" ), Meals = new[] { "Foo", "Bar" } };
			MongoHelper.Collection.InsertOne( menu );
			var command = new TodayCommand( null, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"Today is the *{DateTime.Today:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}", response );
		}
	}
}
