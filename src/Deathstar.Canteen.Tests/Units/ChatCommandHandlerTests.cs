using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ChatCommandHandlerTests
	{
		private readonly IChatResponseRepository chatResponseRepository;
		private readonly ChatCommandHandler commandHandler;
		private readonly ISlackbot slackbot;

		public ChatCommandHandlerTests()
		{
			chatResponseRepository = Substitute.For<IChatResponseRepository>();
			slackbot = Substitute.For<ISlackbot>();
			commandHandler = new ChatCommandHandler(chatResponseRepository, slackbot);
		}

		[Theory]
		[InlineData("foo", "bar")]
		[InlineData("[fF]oo", "bar")]
		[InlineData("[fF]oo", "[fF]oo")]
		[InlineData(".*", "Foobar")]
		public async Task TheHandleMethodShouldAddResponseAsync(string regex, string response)
		{
			// Act
			await commandHandler.HandleAsync($"add {regex} {response}", string.Empty, default);

			// Assert
			await chatResponseRepository.Received().AddAsync(regex, response);
			slackbot.Received().SendMessage(string.Empty, "I added your chat response to my AI.");
		}

		[Theory]
		[InlineData("foo", "bar", "You foo!")]
		[InlineData("[fF]oo", "bar", "You Foo!")]
		public async Task TheHandleMethodShouldChatAsync(string regex, string response, string arguments)
		{
			// Arrange
			var chatResponses = new[] { new ChatResponse { Regex = regex, Response = response }, };

			chatResponseRepository.GetAsync(CancellationToken.None).Returns(chatResponses);

			// Act
			await commandHandler.HandleAsync(arguments, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodShouldRemoveResponseAsync()
		{
			// Arrange
			chatResponseRepository.RemoveAsync("foo").Returns(true);

			// Act
			await commandHandler.HandleAsync("remove foo", string.Empty, default);

			// Assert
			await chatResponseRepository.Received().RemoveAsync("foo");
			slackbot.Received().SendMessage(string.Empty, "I just forgot your response. Can't remember a thing.");
		}
	}
}
