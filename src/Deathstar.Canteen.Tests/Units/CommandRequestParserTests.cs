using Slackbot;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandRequestParserTests
	{
		[Fact]
		public void TheParseMethodShouldOptionallyIgnoreMessagePrefixIfUsernameIsMentioned()
		{
			// Arrange
			var commandRequestParser = new CommandRequestParser( "foobar" );
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "help"
			};

			// Act
			CommandRequest commandRequest = commandRequestParser.Parse( message );

			// Assert
			Assert.NotNull( commandRequest );
			Assert.Equal( "help", commandRequest.Name );
			Assert.Equal( "", commandRequest.Arguments );
		}

		[Fact]
		public void TheParseMethodShouldNotIgnoreMessagePrefixIfUsernameIsNotMentioned()
		{
			// Arrange
			var commandRequestParser = new CommandRequestParser( "barfoo" );
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "help"
			};

			// Act
			CommandRequest commandRequest = commandRequestParser.Parse( message );

			// Assert
			Assert.Null( commandRequest );
		}

		[Fact]
		public void TheParseMethodShouldIgnoreNotMentionedCommands()
		{
			// Arrange
			var commandRequestParser = new CommandRequestParser( "foo" );
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "bar" },
				Text = "<@12345> help"
			};

			// Act
			CommandRequest commandRequest = commandRequestParser.Parse( message );

			// Assert
			Assert.Null( commandRequest );
		}

		[Fact]
		public void TheParseMethodShouldParseAguments()
		{
			// Arrange
			var commandRequestParser = new CommandRequestParser( "foobar" );
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "<@12345> help help"
			};

			// Act
			CommandRequest commandRequest = commandRequestParser.Parse( message );

			// Assert
			Assert.NotNull( commandRequest );
			Assert.Equal( "help", commandRequest.Name );
			Assert.Equal( "help", commandRequest.Arguments );
		}

		[Fact]
		public void TheParseMethodShouldReturnAComandRequest()
		{
			// Arrange
			var commandRequestParser = new CommandRequestParser( "foobar" );
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "<@12345> help"
			};

			// Act
			CommandRequest commandRequest = commandRequestParser.Parse( message );

			// Assert
			Assert.NotNull( commandRequest );
		}
	}
}
