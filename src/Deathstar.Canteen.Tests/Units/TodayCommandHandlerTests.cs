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
	public class TodayCommandHandlerTests
	{
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ICommandHandler commandHandler;

		public TodayCommandHandlerTests() => commandHandler = new TodayCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldReturnErrorNoticeWhenNoMenuisPresentAsync()
		{
			// Act
			await commandHandler.HandleAsync(null, string.Empty, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served today!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTodaysMenuAsync()
		{
			// Arrange
			var menu = new Menu { Date = DateTime.Today.ToString("yyyyMMdd"), Meals = new[] { "Foo", "Bar" } };

			menuRepository.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(menu);

			// Act
			await commandHandler.HandleAsync(null, string.Empty, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(
				string.Empty,
				$"Today is the *{DateTime.Today:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}");
		}
	}
}
