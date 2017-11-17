using System;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class HelpCommandTests
	{
		[Theory]
		[InlineData( "help" )]
		[InlineData( "HELP" )]
		[InlineData( "hELp" )]
		public async Task TheHandleMethodShouldNotCareAboutCaseSensitivity( string commandName )
		{
			// Arrange
			var command = new HelpCommand( commandName, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "The *help* command will return a list of supported commands.", response );
		}

		[Fact]
		public async Task TheHandleMethodWithAddCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "add", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"The *add* command can be used to add something to the menu.{Environment.NewLine}{Environment.NewLine}Example: `add 01012017 Foobar`", response );
		}

		[Fact]
		public async Task TheHandleMethodWithClearCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "clear", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"The *clear* command can be used to clear the menu on a given date.{Environment.NewLine}{Environment.NewLine}Example: `clear 01012017`", response );
		}

		[Fact]
		public async Task TheHandleMethodWithDayAfterTomorrowCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "dayaftertomorrow", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "The *dayaftertomorrow* command will return a list of the day after tomorrow's meals.", response );
		}

		[Fact]
		public async Task TheHandleMethodWithHelpCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "help", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "The *help* command will return a list of supported commands.", response );
		}

		[Fact]
		public async Task TheHandleMethodWithImportCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "import", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal(
				$"The *import* command can be used to import a json based list of menus.{Environment.NewLine}{Environment.NewLine}Example: `import https://some.url/endpoint`",
				response );
		}

		[Fact]
		public async Task TheHandleMethodWithNextCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "next", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"The *next* command will return a list of menus of the next days.{Environment.NewLine}{Environment.NewLine}Example: `next 5`", response );
		}

		[Fact]
		public async Task TheHandleMethodWithNoCommandNameShouldReturnGeneralHelpMessage()
		{
			// Arrange
			string generalHelpText = $"The following commands are available:{Environment.NewLine}"
				+ $"  *help*{Environment.NewLine}"
				+ $"  *today*{Environment.NewLine}"
				+ $"  *tomorrow*{Environment.NewLine}"
				+ $"  *dayaftertomorrow*{Environment.NewLine}"
				+ $"  *next*{Environment.NewLine}"
				+ $"  *search*{Environment.NewLine}"
				+ $"  *add*{Environment.NewLine}"
				+ $"  *clear*{Environment.NewLine}"
				+ $"  *import*{Environment.NewLine}"
				+ $"  *stats*{Environment.NewLine}"
				+ Environment.NewLine
				+ "Use *help command* for more information about each command.";
			var command = new HelpCommand( null, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( generalHelpText, response );
		}

		[Fact]
		public async Task TheHandleMethodWithSearchCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "search", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( $"The *search* command will query future meals and displays the found menus.{Environment.NewLine}{Environment.NewLine}Example: `search Foobar`",
				response );
		}

		[Fact]
		public async Task TheHandleMethodWithStatsCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "stats", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "The *stats* command will display internal statistics of the canteen.", response );
		}

		[Fact]
		public async Task TheHandleMethodWithTodayCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "today", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "The *today* command will return a list of today's meals.", response );
		}

		[Fact]
		public async Task TheHandleMethodWithTomorrowCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "tomorrow", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( "The *tomorrow* command will return a list of tomorrow's meals.", response );
		}

		[Fact]
		public async Task TheHandleMethodWithUnknownCommandNameShouldReturnGeneralHelpMessage()
		{
			// Arrange
			string generalHelpText = $"The following commands are available:{Environment.NewLine}"
				+ $"  *help*{Environment.NewLine}"
				+ $"  *today*{Environment.NewLine}"
				+ $"  *tomorrow*{Environment.NewLine}"
				+ $"  *dayaftertomorrow*{Environment.NewLine}"
				+ $"  *next*{Environment.NewLine}"
				+ $"  *search*{Environment.NewLine}"
				+ $"  *add*{Environment.NewLine}"
				+ $"  *clear*{Environment.NewLine}"
				+ $"  *import*{Environment.NewLine}"
				+ $"  *stats*{Environment.NewLine}"
				+ Environment.NewLine
				+ "Use *help command* for more information about each command.";
			var command = new HelpCommand( "unkwnown", MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Equal( generalHelpText, response );
		}
	}
}
