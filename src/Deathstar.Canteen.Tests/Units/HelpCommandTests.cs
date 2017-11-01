using System;
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
		public void TheHandleMethodShouldNotCareAboutCaseSensitivity( string commandName )
		{
			// Arrange
			var command = new HelpCommand( commandName, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "The *help* command will return a list of supported commands.", response );
		}

		[Fact]
		public void TheHandleMethodWithAddCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "add", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"The *add* command can be used to add something to the menu.{Environment.NewLine}{Environment.NewLine}Example: `add 01012017 Foobar`", response );
		}

		[Fact]
		public void TheHandleMethodWithClearCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "clear", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"The *clear* command can be used to clear the menu on a given date.{Environment.NewLine}{Environment.NewLine}Example: `clear 01012017", response );
		}

		[Fact]
		public void TheHandleMethodWithDayAfterTomorrowCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "dayaftertomorrow", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "The *dayaftertomorrow* command will return a list of the day after tomorrow's meals.", response );
		}

		[Fact]
		public void TheHandleMethodWithHelpCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "help", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "The *help* command will return a list of supported commands.", response );
		}

		[Fact]
		public void TheHandleMethodWithNextCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "next", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( $"The *next* command will return a list of menus of the next days.{Environment.NewLine}{Environment.NewLine}Example: `next 5", response );
		}

		[Fact]
		public void TheHandleMethodWithNoCommandNameShouldReturnGeneralHelpMessage()
		{
			// Arrange
			string generalHelpText = $"The following commands are available:{Environment.NewLine}"
				+ $"  *help*{Environment.NewLine}"
				+ $"  *today*{Environment.NewLine}"
				+ $"  *tomorrow*{Environment.NewLine}"
				+ $"  *dayaftertomorrow*{Environment.NewLine}"
				+ $"  *next*{Environment.NewLine}"
				+ $"  *add*{Environment.NewLine}"
				+ $"  *clear*{Environment.NewLine}"
				+ Environment.NewLine
				+ "Use *help command* for more information about each command.";
			var command = new HelpCommand( null, MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( generalHelpText, response );
		}

		[Fact]
		public void TheHandleMethodWithTodayCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "today", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "The *today* command will return a list of today's meals.", response );
		}

		[Fact]
		public void TheHandleMethodWithTomorrowCommandNameShouldReturnDetailedHelpMessage()
		{
			// Arrange
			var command = new HelpCommand( "tomorrow", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( "The *tomorrow* command will return a list of tomorrow's meals.", response );
		}

		[Fact]
		public void TheHandleMethodWithUnknownCommandNameShouldReturnGeneralHelpMessage()
		{
			// Arrange
			string generalHelpText = $"The following commands are available:{Environment.NewLine}"
				+ $"  *help*{Environment.NewLine}"
				+ $"  *today*{Environment.NewLine}"
				+ $"  *tomorrow*{Environment.NewLine}"
				+ $"  *dayaftertomorrow*{Environment.NewLine}"
				+ $"  *next*{Environment.NewLine}"
				+ $"  *add*{Environment.NewLine}"
				+ $"  *clear*{Environment.NewLine}"
				+ Environment.NewLine
				+ "Use *help command* for more information about each command.";
			var command = new HelpCommand( "unkwnown", MongoHelper.Client );

			// Act
			string response = command.Handle();

			// Assert
			Assert.Equal( generalHelpText, response );
		}
	}
}
