using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Slackbot;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandMessageParserTests
	{
		private readonly ICommandMessageParser commandRequestParser = new CommandMessageParser();

		[Fact]
		public void TheParseMethodShouldOptionallyIgnoreMessagePrefixIfUsernameIsMentioned()
		{
			// Arrange
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
