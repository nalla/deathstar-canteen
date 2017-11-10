using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using MongoDB.Driver;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class AddCommandTests
	{
		public AddCommandTests() => MongoHelper.Clear();

		[Theory]
		[InlineData( null )]
		[InlineData( "" )]
		[InlineData( "01012017" )]
		[InlineData( "01.01.2017" )]
		[InlineData( "01012017 " )]
		[InlineData( "01.01.2017 " )]
		[InlineData( "abc" )]
		[InlineData( " abc" )]
		public async Task TheHandleMethodShouldExpectValidInput( string arguments )
		{
			// Arrange
			var command = new AddCommand( arguments, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToExistingMenu()
		{
			// Arrange
			var menu = new Menu
			{
				Date = DateTime.Today.ToString( "yyyyMMdd" ),
				Meals = new[] { "Foo" }
			};
			MongoHelper.Collection.InsertOne( menu );
			var command = new AddCommand( $"{DateTime.Today:ddMMyyyy} Bar", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"I added _Bar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foo", menu.Meals );
			Assert.Contains( "Bar", menu.Meals );
			Assert.Equal( 2, menu.Meals.Length );
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToTodaysMenuUsingTheDotNotation()
		{
			// Arrange
			var command = new AddCommand( $"{DateTime.Today:dd.MM.yyyy} Foobar", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			Menu menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foobar", menu.Meals );
			Assert.Single( menu.Meals );
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToTodaysMenuUsingTheNoDotNotation()
		{
			// Arrange
			var command = new AddCommand( $"{DateTime.Today:ddMMyyyy} Foobar", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			Menu menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foobar", menu.Meals );
			Assert.Single( menu.Meals );
		}

		[Fact]
		public async Task TheHandleMethodShouldNotAddSameMealTwice()
		{
			// Arrange
			var menu = new Menu
			{
				Date = DateTime.Today.ToString( "yyyyMMdd" ),
				Meals = new[] { "Foo" }
			};
			MongoHelper.Collection.InsertOne( menu );
			var command = new AddCommand( $"{DateTime.Today:ddMMyyyy} Foo", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"_Foo_ is already on the menu on *{DateTime.Today:dd.MM.yyyy}*!", response );
			menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foo", menu.Meals );
			Assert.Single( menu.Meals );
		}
	}
}
