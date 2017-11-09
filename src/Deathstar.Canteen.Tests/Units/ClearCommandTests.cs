using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using MongoDB.Driver;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ClearCommandTests
	{
		public ClearCommandTests() => MongoHelper.Clear();

		[Theory]
		[InlineData( null )]
		[InlineData( "" )]
		[InlineData( "0101201" )]
		[InlineData( "01.01.201" )]
		[InlineData( "abc" )]
		[InlineData( " abc" )]
		public async Task TheHandleMethoudShouldExpectValidInput( string arguments )
		{
			// Arrange
			var command = new ClearCommand( arguments, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldClearExistingMenu()
		{
			// Arrange
			var menu = new Menu
			{
				Date = DateTime.Today.ToString( "yyyyMMdd" ),
				Meals = new[] { "Foo" }
			};
			MongoHelper.Collection.InsertOne( menu );
			var command = new ClearCommand( $"{DateTime.Today:ddMMyyyy}", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"I cleared the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).SingleOrDefault();
			Assert.Null( menu );
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNothingWasCleared()
		{
			// Arrange
			var command = new ClearCommand( $"{DateTime.Today:dd.MM.yyyy}", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"There is no menu on *{DateTime.Today:dd.MM.yyyy}*!", response );
		}
	}
}
