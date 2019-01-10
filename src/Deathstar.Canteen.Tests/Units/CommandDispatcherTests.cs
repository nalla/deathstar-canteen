using System.Threading.Tasks;
using Deathstar.Canteen.Services;
using Deathstar.Canteen.Tests.Mocks;
using Slackbot;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandDispatcherTests
	{
		private readonly ICommandDispatcher commandDispatcher;
		private readonly FakeCommandHandler defaultCommandHandler = new FakeCommandHandler("chat");
		private readonly FakeCommandHandler fakeCommandHandler = new FakeCommandHandler("fake");

		public CommandDispatcherTests() => commandDispatcher = new CommandDispatcher(new[] { fakeCommandHandler, defaultCommandHandler });

		[Fact]
		public async Task TheDispatchMethodShouldCreateAndDispatchACommandAsync()
		{
			// Arrange
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "<@12345> fake",
			};

			// Act
			await commandDispatcher.DispatchAsync(message, default);

			// Assert
			Assert.Equal(string.Empty, fakeCommandHandler.ReceivedArguments);
		}

		[Fact]
		public async Task TheDispatchMethodShouldOptionallyIgnoreMessagePrefixIfUsernameIsMentionedAsync()
		{
			// Arrange
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "fake",
			};

			// Act
			await commandDispatcher.DispatchAsync(message, default);

			// Assert
			Assert.Equal(string.Empty, fakeCommandHandler.ReceivedArguments);
		}

		[Fact]
		public async Task TheDispatchMethodShouldParseAgumentsAsync()
		{
			// Arrange
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "<@12345> fake help",
			};

			// Act
			await commandDispatcher.DispatchAsync(message, default);

			// Assert
			Assert.Equal("help", fakeCommandHandler.ReceivedArguments);
		}

		[Fact]
		public async Task TheDispatchMethodShouldUseChatCommandAsDefaultAsync()
		{
			// Arrange
			var message = new OnMessageArgs
			{
				MentionedUsers = new[] { "foobar" },
				Text = "foobar",
			};

			// Act
			await commandDispatcher.DispatchAsync(message, default);

			// Assert
			Assert.Equal("foobar", defaultCommandHandler.ReceivedArguments);
		}
	}
}
