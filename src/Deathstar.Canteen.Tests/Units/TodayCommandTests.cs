using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class TodayCommandTests
	{
		[Fact]
		public async Task TheHandleMethodShouldReturnErrorNoticeWhenNoMenuisPresentAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new TodayCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage(null, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "I don't know which meals are being served today!");
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnTodaysMenuAsync()
		{
			// Arrange
			var menu = new Menu { Date = DateTime.Today.ToString("yyyyMMdd"), Meals = new[] { "Foo", "Bar" } };
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new TodayCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage(null, string.Empty);

			menuCollection.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(menu);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(
				string.Empty,
				$"Today is the *{DateTime.Today:dd.MM.yyyy}* and the meals are:{Environment.NewLine}{menu}");
		}
	}
}
