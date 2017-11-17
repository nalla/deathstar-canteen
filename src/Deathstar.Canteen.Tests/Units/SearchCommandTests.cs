using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class SearchCommandTests
	{
		public SearchCommandTests() => MongoHelper.Clear();

		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnFromToday()
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
					Date = DateTime.Today.AddDays( -1 ).ToString( "yyyyMMdd" ),
					Meals = new[] { "Foo" }
				}
			} );
			var command = new SearchCommand( "Foo", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo", response );
		}

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
			var command = new SearchCommand( "Foo", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnResultsSmallerThanTen()
		{
			// Arrange
			for( int i = 1; i < 12; i++ )
			{
				MongoHelper.Collection.InsertOne( new Menu
				{
					Date = DateTime.Today.AddDays( i ).ToString( "yyyyMMdd" ),
					Meals = new[] { "Foo" }
				} );
			}

			var command = new SearchCommand( "Foo", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "I found more than 10 menus. Please be more precise.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidCommandArguments()
		{
			// Arrange
			var command = new SearchCommand( "", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutMissingCommandArguments()
		{
			// Arrange
			var command = new SearchCommand( null, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNoDataWasFound()
		{
			// Arrange
			var command = new SearchCommand( "foo", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "I found nothing.", response );
		}
	}
}
