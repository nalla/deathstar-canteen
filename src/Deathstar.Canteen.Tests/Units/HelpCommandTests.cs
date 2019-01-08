using System;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class HelpCommandTests
	{
		private readonly HelpCommand command;
		private readonly ISlackbot slackbot;

		public HelpCommandTests()
		{
			var serviceProvider = Substitute.For<IServiceProvider>();
			serviceProvider.GetService(typeof(ICommandFactory)).Returns(new CommandFactory(new[] { new FakeCommand() }));

			slackbot = Substitute.For<ISlackbot>();
			command = new HelpCommand(slackbot, serviceProvider);
		}

		[Theory]
		[InlineData("fake")]
		[InlineData("FAKE")]
		[InlineData("fAKe")]
		public async Task TheHandleMethodShouldNotCareAboutCaseSensitivityAsync(string commandName)
		{
			// Arrange
			var commandMessage = new FakeCommandMessage(commandName, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

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
			var commandMessage = new FakeCommandMessage(null, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

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
			var commandMessage = new FakeCommandMessage("unkwnown", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}
	}
}
