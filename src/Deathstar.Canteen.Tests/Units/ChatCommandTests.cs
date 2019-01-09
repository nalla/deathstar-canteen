using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ChatCommandTests
	{
		private readonly ChatCommand command;
		private readonly ISlackbot slackbot;
		private readonly IChatResponseCollection chatResponseCollection;

		public ChatCommandTests()
		{
			chatResponseCollection = Substitute.For<IChatResponseCollection>();
			slackbot = Substitute.For<ISlackbot>();
			command = new ChatCommand(chatResponseCollection, slackbot);
		}

		[Theory]
		[InlineData("foo", "bar", "You foo!")]
		[InlineData("[fF]oo", "bar", "You Foo!")]
		public async Task TheHandleMethodShouldChatAsync(string regex, string response, string arguments)
		{
			// Arrange
			var chatResponses = new[] { new ChatResponse { Regex = regex, Response = response }, };
			var commandMessage = new FakeCommandMessage(arguments, string.Empty);

			chatResponseCollection.GetAsync(CancellationToken.None).Returns(chatResponses);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Theory]
		[InlineData("foo", "bar")]
		[InlineData("[fF]oo", "bar")]
		[InlineData("[fF]oo", "[fF]oo")]
		[InlineData(".*", "Foobar")]
		public async Task TheHandleMethodShouldAddResponseAsync(string regex, string response)
		{
			// Arrange
			var commandMessage = new FakeCommandMessage($"add {regex} {response}", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			await chatResponseCollection.Received().AddAsync(regex, response);
			slackbot.Received().SendMessage(string.Empty, "I added your chat response to my AI.");
		}

		[Fact]
		public async Task TheHandleMethodShouldRemoveResponseAsync()
		{
			// Arrange
			var commandMessage = new FakeCommandMessage("remove foo", string.Empty);
			chatResponseCollection.RemoveAsync("foo").Returns(true);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			await chatResponseCollection.Received().RemoveAsync("foo");
			slackbot.Received().SendMessage(string.Empty, "I just forgot your response. Can't remember a thing.");
		}
	}
}
