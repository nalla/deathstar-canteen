using System;
using System.Collections.Generic;
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
	public class SearchCommandHandlerTests
	{
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ICommandHandler commandHandler;

		public SearchCommandHandlerTests() => commandHandler = new SearchCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnMenusAsync()
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
			await commandHandler.HandleAsync("Foo", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo");
		}

		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnResultsSmallerThanTenAsync()
		{
			// Arrange
			var menus = new List<Menu>();

			for (var i = 1; i < 12; i++)
			{
				menus.Add(new Menu { Date = DateTime.Today.AddDays(i).ToString("yyyyMMdd"), Meals = new[] { "Foo" } });
			}

			menuRepository.ToListAsync(Arg.Any<FilterDefinition<Menu>>(), CancellationToken.None).Returns(menus);

			// Act
			await commandHandler.HandleAsync("Foo", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I found more than 10 menus. Please be more precise.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidCommandArgumentsAsync()
		{
			// Act
			await commandHandler.HandleAsync(string.Empty, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutMissingCommandArgumentsAsync()
		{
			// Act
			await commandHandler.HandleAsync(null, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNoDataWasFoundAsync()
		{
			// Act
			await commandHandler.HandleAsync("foo", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I found nothing.");
		}
	}
}
