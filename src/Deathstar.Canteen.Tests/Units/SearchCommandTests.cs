using System;
using System.Collections.Generic;
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
	public class SearchCommandTests
	{
		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnMenusAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new SearchCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("Foo", string.Empty);

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

		[Fact]
		public async Task TheHandleMethodShouldOnlyReturnResultsSmallerThanTenAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new SearchCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("Foo", string.Empty);
			var menus = new List<Menu>();

			for (var i = 1; i < 12; i++)
			{
				menus.Add(new Menu { Date = DateTime.Today.AddDays(i).ToString("yyyyMMdd"), Meals = new[] { "Foo" } });
			}

			menuCollection.ToListAsync(Arg.Any<FilterDefinition<Menu>>(), CancellationToken.None).Returns(menus);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I found more than 10 menus. Please be more precise.");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidCommandArgumentsAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new SearchCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage(string.Empty, string.Empty);

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
			var command = new SearchCommand(menuCollection, slackbot);
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
			var command = new SearchCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage("foo", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I found nothing.");
		}
	}
}
