using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class NextCommandTests
	{
		public NextCommandTests() => MongoHelper.Clear();

		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnQueriedDays()
		{
			// Arrange
			MongoHelper.Collection.InsertMany( new[]
			{
				new Menu
				{
					Date = DateTime.Today.ToString( "yyyyMMdd" ),
					Meals = new[] { "Foo" }
				},
				new Menu
				{
					Date = DateTime.Today.AddDays( 1 ).ToString( "yyyyMMdd" ),
					Meals = new[] { "Bar" }
				}
			} );
			var command = new NextCommand( "1", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnAMaxiumumOfSevenDays()
		{
			// Arrange
			var command = new NextCommand( "10", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "I don't know which meals are being served the next 7 days!", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnAMinimumOfOneDay()
		{
			// Arrange
			var command = new NextCommand( "0", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "I don't know which meals are being served the next 1 days!", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidCommandArguments()
		{
			// Arrange
			var command = new NextCommand( "Foobar", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutMissingCommandArguments()
		{
			// Arrange
			var command = new NextCommand( null, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNoDataWasFound()
		{
			// Arrange
			var command = new NextCommand( "1", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "I don't know which meals are being served the next 1 days!", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTodaysAndTomorrowsMenu()
		{
			// Arrange
			MongoHelper.Collection.InsertMany( new[]
			{
				new Menu
				{
					Date = DateTime.Today.ToString( "yyyyMMdd" ),
					Meals = new[] { "Foo" }
				},
				new Menu
				{
					Date = DateTime.Today.AddDays( 1 ).ToString( "yyyyMMdd" ),
					Meals = new[] { "Bar" }
				}
			} );
			var command = new NextCommand( "2", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo"
				+ Environment.NewLine
				+ Environment.NewLine
				+ $"On *{DateTime.Today.AddDays( 1 ):dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Bar", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTodaysMenu()
		{
			// Arrange
			MongoHelper.Collection.InsertOne( new Menu
			{
				Date = DateTime.Today.ToString( "yyyyMMdd" ),
				Meals = new[] { "Foo" }
			} );
			var command = new NextCommand( "1", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo", response );
		}
	}
}
