using System;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class HelpCommandTests
	{
		[Theory]
		[InlineData("help")]
		[InlineData("HELP")]
		[InlineData("hELp")]
		public async Task TheHandleMethodShouldNotCareAboutCaseSensitivityAsync(string commandName)
		{
			// Arrange
			const string response = "The *help* command will return a list of supported commands.";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage(commandName, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithAddCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			string response = $"The *add* command can be used to add something to the menu.{Environment.NewLine}{Environment.NewLine}Example: `add 01012017 Foobar`";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("add", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithClearCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			string response = $"The *clear* command can be used to clear the menu on a given date.{Environment.NewLine}{Environment.NewLine}Example: `clear 01012017`";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("clear", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithDayAfterTomorrowCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			const string response = "The *dayaftertomorrow* command will return a list of the day after tomorrow's meals.";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("dayaftertomorrow", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithHelpCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			const string response = "The *help* command will return a list of supported commands.";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("help", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithImportCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			string response = "The *import* command can be used to import a json based list of menus." +
				$"{Environment.NewLine}{Environment.NewLine}Example: `import https://some.url/endpoint`";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("import", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithNextCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			string response = $"The *next* command will return a list of menus of the next days.{Environment.NewLine}{Environment.NewLine}Example: `next 5`";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("next", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithNoCommandNameShouldReturnGeneralHelpMessageAsync()
		{
			// Arrange
			string response = $"The following commands are available:{Environment.NewLine}"
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
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage(null, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithSearchCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			string response = $"The *search* command will query future meals and displays the found menus.{Environment.NewLine}{Environment.NewLine}Example: `search Foobar`";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("search", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithStatsCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			var response = "The *stats* command will display internal statistics of the canteen.";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("stats", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithTodayCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			const string response = "The *today* command will return a list of today's meals.";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("today", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodWithTomorrowCommandNameShouldReturnDetailedHelpMessageAsync()
		{
			// Arrange
			var response = "The *tomorrow* command will return a list of tomorrow's meals.";
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("tomorrow", string.Empty);

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
			var slackbot = Substitute.For<ISlackbot>();
			var command = new HelpCommand(slackbot);
			var commandMessage = new FakeCommandMessage("unkwnown", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, response);
		}
	}
}
