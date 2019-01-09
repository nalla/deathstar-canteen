using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ClearCommandTests
	{
		private readonly IMenuCollection menuCollection = Substitute.For<IMenuCollection>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ICommand command;

		public ClearCommandTests() => command = new ClearCommand(menuCollection, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldClearExistingMenuAsync()
		{
			// Arrange
			var commandMessage = new FakeCommandMessage($"{DateTime.Today:ddMMyyyy}", string.Empty);

			menuCollection.DeleteOneAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(1);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, $"I cleared the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNothingWasClearedAsync()
		{
			// Arrange
			var commandMessage = new FakeCommandMessage($"{DateTime.Today:dd.MM.yyyy}", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, $"There is no menu on *{DateTime.Today:dd.MM.yyyy}*!");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("0101201")]
		[InlineData("01.01.201")]
		[InlineData("abc")]
		[InlineData(" abc")]
		public async Task TheHandleMethoudShouldExpectValidInputAsync(string arguments)
		{
			// Arrange
			var commandMessage = new FakeCommandMessage(arguments, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}
	}
}
