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
	public class TomorrowCommandHandlerTests
	{
		private readonly ICommandHandler commandHandler;
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();

		public TomorrowCommandHandlerTests() => commandHandler = new TomorrowCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldReturnErrorNoticeWhenNoMenuisPresentAsync()
		{
			// Act
			await commandHandler.HandleAsync(null, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served tomorrow!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTomorrowsMenuAsync()
		{
			// Arrange
			var menu = new Menu { Date = DateTime.Today.AddDays(1).ToString("yyyyMMdd"), Meals = new[] { "Foo", "Bar" } };

			menuRepository.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(menu);

			// Act
			await commandHandler.HandleAsync(null, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(
				string.Empty,
				$"Tomorrow is the *{DateTime.Today.AddDays(1):dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}");
		}
	}
}
