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
	public class NextCommandHandlerTests
	{
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ICommandHandler commandHandler;

		public NextCommandHandlerTests() => commandHandler = new NextCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldReturnAMaxiumumOfSevenDaysAsync()
		{
			// Act
			await commandHandler.HandleAsync("10", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served the next 7 days!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnAMinimumOfOneDayAsync()
		{
			// Act
			await commandHandler.HandleAsync("0", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served the next 1 days!");
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
					new Menu
					{
						Date = DateTime.Today.AddDays(1).ToString("yyyyMMdd"),
						Meals = new[] { "Bar" },
					},
				}.ToList());

			// Act
			await commandHandler.HandleAsync("2", string.Empty, default);

			// Assert
			var response = $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo"
				+ Environment.NewLine
				+ Environment.NewLine
				+ $"On *{DateTime.Today.AddDays(1):dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Bar";
			slackbot.Received().SendMessage(string.Empty, response);
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidCommandArgumentsAsync()
		{
			// Act
			await commandHandler.HandleAsync("Foobar", string.Empty, default);

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
			await commandHandler.HandleAsync("1", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served the next 1 days!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTodaysMenuAsync()
		{
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
			await commandHandler.HandleAsync("1", string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo");
		}
	}
}
