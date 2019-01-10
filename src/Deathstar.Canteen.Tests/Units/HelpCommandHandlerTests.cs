using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deathstar.Canteen.Services;
using Deathstar.Canteen.Tests.Mocks;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class HelpCommandHandlerTests
	{
		private readonly ICommandHandler commandHandler;
		private readonly ISlackbot slackbot;

		public HelpCommandHandlerTests()
		{
			var serviceProvider = Substitute.For<IServiceProvider>();
			serviceProvider.GetService(typeof(IEnumerable<ICommandHandler>)).Returns(new[] { new FakeCommandHandler("fake") });

			slackbot = Substitute.For<ISlackbot>();
			commandHandler = new HelpCommandHandler(slackbot, serviceProvider);
		}

		[Theory]
		[InlineData("fake")]
		[InlineData("FAKE")]
		[InlineData("fAKe")]
		public async Task TheHandleMethodShouldNotCareAboutCaseSensitivityAsync(string commandName)
		{
			// Act
			await commandHandler.HandleAsync(commandName, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "A fake command");
		}

		[Fact]
		public async Task TheHandleMethodWithNoCommandNameShouldReturnGeneralHelpMessageAsync()
		{
			// Arrange
			string response = $"The following commands are available:{Environment.NewLine}"
				+ $"  *fake*{Environment.NewLine}"
				+ Environment.NewLine
				+ "Use *help command* for more information about each command.";

			// Act
			await commandHandler.HandleAsync(null, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithUnknownCommandNameShouldReturnGeneralHelpMessageAsync()
		{
			// Arrange
			string response = $"The following commands are available:{Environment.NewLine}"
				+ $"  *fake*{Environment.NewLine}"
				+ Environment.NewLine
				+ "Use *help command* for more information about each command.";

			// Act
			await commandHandler.HandleAsync("unknown", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}
	}
}
