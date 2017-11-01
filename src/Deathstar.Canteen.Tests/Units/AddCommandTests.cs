using System;
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
		public void TheHandleMethoudShouldExpectValidInput( string arguments )
		{
			// Arrange
			var command = new AddCommand( arguments, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "You need to provide some valid input.", response );
		}

		[Fact]
		public void TheHandleMethodShouldAddMealToExistingMenu()
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
			string response = command.Handle();

			// Assert
			Assert.Equal( $"I added _Bar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foo", menu.Meals );
			Assert.Contains( "Bar", menu.Meals );
			Assert.Equal( 2, menu.Meals.Length );
		}

		[Fact]
		public void TheHandleMethodShouldAddMealToTodaysMenuUsingTheDotNotation()
		{
			// Arrange
			var command = new AddCommand( $"{DateTime.Today:dd.MM.yyyy} Foobar", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			Menu menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foobar", menu.Meals );
			Assert.Single( menu.Meals );
		}

		[Fact]
		public void TheHandleMethodShouldAddMealToTodaysMenuUsingTheNoDotNotation()
		{
			// Arrange
			var command = new AddCommand( $"{DateTime.Today:ddMMyyyy} Foobar", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.", response );
			Menu menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foobar", menu.Meals );
			Assert.Single( menu.Meals );
		}

		[Fact]
		public void TheHandleMethodShouldNotAddSameMealTwice()
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
			string response = command.Handle();

			// Assert
			Assert.Equal( $"_Foo_ is already on the menu on *{DateTime.Today:dd.MM.yyyy}*!", response );
			menu = MongoHelper.Collection.Find( x => x.Date == DateTime.Today.ToString( "yyyyMMdd" ) ).Single();
			Assert.Contains( "Foo", menu.Meals );
			Assert.Single( menu.Meals );
		}
	}
}
