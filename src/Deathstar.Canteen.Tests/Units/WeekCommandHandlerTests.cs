using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class WeekCommandHandlerTests
	{
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ICommandHandler commandHandler;

		public WeekCommandHandlerTests() => commandHandler = new WeekCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeIfNothingIsFoundAsync()
		{
			// Act
			await commandHandler.HandleAsync(string.Empty, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served this week!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnMenusAsync()
		{
			// Arrange
			menuRepository.ToListAsync(Arg.Any<FilterDefinition<Menu>>(), CancellationToken.None).Returns(
				new[]
				{
					new Menu
					{
						Date = DateTime.Today.ToString("yyyyMMdd"),
						Meals = new[] { "Foo" },
					},
				}.ToList());

			// Act
			await commandHandler.HandleAsync(string.Empty, string.Empty, default);

			// Assert
			var response = $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo";
			slackbot.Received().SendMessage(string.Empty, response);
		}
	}
}
