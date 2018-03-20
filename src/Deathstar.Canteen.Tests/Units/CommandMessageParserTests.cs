using Deathstar.Canteen.Commands;
using Slackbot;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandMessageParserTests
	{
		[Fact]
		public void TheParseMethodShouldOptionallyIgnoreMessagePrefixIfUsernameIsMentioned()
		{
			// Arrange
			var commandRequestParser = new CommandMessageParser();
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "help",
			};

			// Act
			CommandMessage commandMessage = commandRequestParser.Parse(message);

			// Assert
			Assert.NotNull(commandMessage);
			Assert.Equal("help", commandMessage.Name);
			Assert.Equal(string.Empty, commandMessage.Arguments);
		}

		[Fact]
		public void TheParseMethodShouldParseAguments()
		{
			// Arrange
			var commandRequestParser = new CommandMessageParser();
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "<@12345> help help",
			};

			// Act
			CommandMessage commandMessage = commandRequestParser.Parse(message);

			// Assert
			Assert.NotNull(commandMessage);
			Assert.Equal("help", commandMessage.Name);
			Assert.Equal("help", commandMessage.Arguments);
		}

		[Fact]
		public void TheParseMethodShouldReturnAComandRequest()
		{
			// Arrange
			var commandRequestParser = new CommandMessageParser();
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "<@12345> help",
			};

			// Act
			CommandMessage commandMessage = commandRequestParser.Parse(message);

			// Assert
			Assert.NotNull(commandMessage);
		}
	}
}
