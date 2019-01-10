using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ClearCommandHandlerTests
	{
		private readonly ICommandHandler commandHandler;
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();

		public ClearCommandHandlerTests() => commandHandler = new ClearCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldClearExistingMenuAsync()
		{
			// Arrange
			menuRepository.DeleteOneAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(1);

			// Act
			await commandHandler.HandleAsync($"{DateTime.Today:ddMMyyyy}", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, $"I cleared the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNothingWasClearedAsync()
		{
			// Act
			await commandHandler.HandleAsync($"{DateTime.Today:dd.MM.yyyy}", string.Empty, default);

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
			// Act
			await commandHandler.HandleAsync(arguments, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}
	}
}
