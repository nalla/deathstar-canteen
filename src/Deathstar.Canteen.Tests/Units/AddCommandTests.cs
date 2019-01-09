using System;
using System.Linq;
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
	public class AddCommandTests
	{
		private readonly IMenuCollection menuCollection = Substitute.For<IMenuCollection>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ICommand command;

		public AddCommandTests() => command = new AddCommand(menuCollection, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldAddMealToExistingMenuAsync()
		{
			// Arrange
			menuCollection.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(
				new Menu
				{
					Date = DateTime.Today.ToString("yyyyMMdd"),
					Meals = new[] { "Foo" },
				});

			var commandMessage = new FakeCommandMessage($"{DateTime.Today:ddMMyyyy} Bar", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			await menuCollection.Received().ReplaceOneAsync(
				Arg.Any<Expression<Func<Menu, bool>>>(),
				Arg.Is<Menu>(
					x => x.Meals.Contains("Foo") && x.Meals.Contains("Bar")));
			slackbot.Received().SendMessage(string.Empty, $"I added _Bar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToTodaysMenuUsingTheDotNotationAsync()
		{
			// Arrange
			var commandMessage = new FakeCommandMessage($"{DateTime.Today:dd.MM.yyyy} Foobar", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			await menuCollection.Received().InsertOneAsync(Arg.Is<Menu>(x => x.Meals.Contains("Foobar")));
			slackbot.Received().SendMessage(string.Empty, $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToTodaysMenuUsingTheNoDotNotationAsync()
		{
			// Arrange
			var commandMessage = new FakeCommandMessage($"{DateTime.Today:ddMMyyyy} Foobar", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			await menuCollection.Received().InsertOneAsync(Arg.Is<Menu>(x => x.Meals.Contains("Foobar")));
			slackbot.Received().SendMessage(string.Empty, $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("01012017")]
		[InlineData("01.01.2017")]
		[InlineData("01012017 ")]
		[InlineData("01.01.2017 ")]
		[InlineData("abc")]
		[InlineData(" abc")]
		public async Task TheHandleMethodShouldExpectValidInputAsync(string arguments)
		{
			// Arrange
			var commandMessage = new FakeCommandMessage(arguments, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}

		[Fact]
		public async Task TheHandleMethodShouldNotAddSameMealTwiceAsync()
		{
			// Arrange
			menuCollection.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(
				new Menu
				{
					Date = DateTime.Today.ToString("yyyyMMdd"),
					Meals = new[] { "Foo" },
				});

			var commandMessage = new FakeCommandMessage($"{DateTime.Today:ddMMyyyy} Foo", string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			await menuCollection.DidNotReceive().InsertOneAsync(Arg.Is<Menu>(x => x.Meals.Contains("Foobar")));
			slackbot.Received().SendMessage(string.Empty, $"_Foo_ is already on the menu on *{DateTime.Today:dd.MM.yyyy}*!");
		}
	}
}
