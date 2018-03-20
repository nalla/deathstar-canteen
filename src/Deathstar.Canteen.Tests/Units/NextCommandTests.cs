using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class NextCommandTests
	{
		[Fact]
		public async Task TheHandleMethodShouldReturnAMaxiumumOfSevenDaysAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("10", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served the next 7 days!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnAMinimumOfOneDayAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("0", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served the next 1 days!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnMenusAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("2", string.Empty);

			menuCollection.ToListAsync(Arg.Any<FilterDefinition<Menu>>(), CancellationToken.None).Returns(
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
			await command.HandleAsync(commandMessage, CancellationToken.None);

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
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("Foobar", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutMissingCommandArgumentsAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage(null, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeWhenNoDataWasFoundAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("1", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served the next 1 days!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTodaysMenuAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new NextCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("1", string.Empty);

			menuCollection.ToListAsync(Arg.Any<FilterDefinition<Menu>>(), CancellationToken.None).Returns(
				new[]
				{
					new Menu
					{
						Date = DateTime.Today.ToString("yyyyMMdd"),
						Meals = new[] { "Foo" },
					},
				}.ToList());

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, $"On *{DateTime.Today:dd.MM.yyyy}* the meals are:{Environment.NewLine}1. Foo");
		}
	}
}
