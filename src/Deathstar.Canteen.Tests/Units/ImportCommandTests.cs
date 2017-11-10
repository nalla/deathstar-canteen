using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Tests.Helpers;
using Flurl.Http.Testing;
using MongoDB.Driver;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ImportCommandTests
	{
		public ImportCommandTests() => MongoHelper.Clear();

		[Theory]
		[InlineData( null )]
		[InlineData( "" )]
		[InlineData( "foobar" )]
		[InlineData( "1234" )]
		[InlineData( ":/foo/bar" )]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidUrl( string arguments )
		{
			// Arrange
			var command = new ImportCommand( arguments, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "You need to provide a well formed url.", response );
		}

		[Fact]
		public async Task TheHandleMethodShouldCheckDateFormat()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "foo", meals = new[] { "foo" } }
				};
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I imported 0 menus.", response );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldCheckThatMealsAreValid()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", " " } }
				};
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I imported 0 menus.", response );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldCheckThatMealsExists()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "" } }
				};
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I imported 0 menus.", response );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldInsertMenuIntoDatabase()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", "bar" } }
				};
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I imported 1 menus.", response );
				Menu menu = MongoHelper.Collection.Find( x => x.Date == "20010101" ).Single();
				Assert.Contains( "foo", menu.Meals );
				Assert.Contains( "bar", menu.Meals );
				Assert.Equal( 2, menu.Meals.Length );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldAcceptWellFormedUrlsFromSlack()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", "bar" } }
				};
				var command = new ImportCommand( "<https://api.myjson.com/bins/1dekrb>", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "https://api.myjson.com/bins/1dekrb" );
				Assert.Equal( "I imported 1 menus.", response );
				Menu menu = MongoHelper.Collection.Find( x => x.Date == "20010101" ).Single();
				Assert.Contains( "foo", menu.Meals );
				Assert.Contains( "bar", menu.Meals );
				Assert.Equal( 2, menu.Meals.Length );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldInsertMenuOnlyOnceIntoDatabase()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", "bar" } },
					new { date = "20010101", meals = new[] { "bar", "foo" } }
				};
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I imported 1 menus.", response );
				Menu menu = MongoHelper.Collection.Find( x => x.Date == "20010101" ).Single();
				Assert.Contains( "foo", menu.Meals );
				Assert.Contains( "bar", menu.Meals );
				Assert.Equal( 2, menu.Meals.Length );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldInsertMultipleMenusIntoDatabase()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", "bar" } },
					new { date = "20010102", meals = new[] { "bar", "foo" } }
				};
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWithJson( importData );

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I imported 2 menus.", response );
				Menu menu1 = MongoHelper.Collection.Find( x => x.Date == "20010101" ).Single();
				Assert.Contains( "foo", menu1.Meals );
				Assert.Contains( "bar", menu1.Meals );
				Assert.Equal( 2, menu1.Meals.Length );
				Menu menu2 = MongoHelper.Collection.Find( x => x.Date == "20010102" ).Single();
				Assert.Contains( "bar", menu2.Meals );
				Assert.Contains( "foo", menu2.Meals );
				Assert.Equal( 2, menu2.Meals.Length );
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutDownloadError()
		{
			using( var httpTest = new HttpTest() )
			{
				// Arrange
				var command = new ImportCommand( "http://localhost/foobar", MongoHelper.Client );
				httpTest.RespondWith();

				// Act
				string response = await command.HandleAsync();

				// Assert
				httpTest.ShouldHaveCalled( "http://localhost/foobar" );
				Assert.Equal( "I got an error when downloading the url you provided.", response );
			}
		}
	}
}
