using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class AddCommandHandlerTests
	{
		private readonly ICommandHandler commandHandler;
		private readonly IMenuRepository menuRepository = Substitute.For<IMenuRepository>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();

		public AddCommandHandlerTests() => commandHandler = new AddCommandHandler(menuRepository, slackbot);

		[Fact]
		public async Task TheHandleMethodShouldAddMealToExistingMenuAsync()
		{
			// Arrange
			menuRepository.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(
				new Menu
				{
					Date = DateTime.Today.ToString("yyyyMMdd"),
					Meals = new[] { "Foo" },
				});

			// Act
			await commandHandler.HandleAsync($"{DateTime.Today:ddMMyyyy} Bar", string.Empty, default);

			// Assert
			await menuRepository.Received().ReplaceOneAsync(
				Arg.Any<Expression<Func<Menu, bool>>>(),
				Arg.Is<Menu>(
					x => x.Meals.Contains("Foo") && x.Meals.Contains("Bar")));
			slackbot.Received().SendMessage(string.Empty, $"I added _Bar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToTodaysMenuUsingTheDotNotationAsync()
		{
			// Act
			await commandHandler.HandleAsync($"{DateTime.Today:dd.MM.yyyy} Foobar", string.Empty, default);

			// Assert
			await menuRepository.Received().InsertOneAsync(Arg.Is<Menu>(x => x.Meals.Contains("Foobar")));
			slackbot.Received().SendMessage(string.Empty, $"I added _Foobar_ to the menu on *{DateTime.Today:dd.MM.yyyy}*.");
		}

		[Fact]
		public async Task TheHandleMethodShouldAddMealToTodaysMenuUsingTheNoDotNotationAsync()
		{
			// Act
			await commandHandler.HandleAsync($"{DateTime.Today:ddMMyyyy} Foobar", string.Empty, default);

			// Assert
			await menuRepository.Received().InsertOneAsync(Arg.Is<Menu>(x => x.Meals.Contains("Foobar")));
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
			// Act
			await commandHandler.HandleAsync(arguments, string.Empty, default);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide some valid input.");
		}

		[Fact]
		public async Task TheHandleMethodShouldNotAddSameMealTwiceAsync()
		{
			// Arrange
			menuRepository.SingleOrDefaultAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(
				new Menu
				{
					Date = DateTime.Today.ToString("yyyyMMdd"),
					Meals = new[] { "Foo" },
				});

			// Act
			await commandHandler.HandleAsync($"{DateTime.Today:ddMMyyyy} Foo", string.Empty, default);

			// Assert
			await menuRepository.DidNotReceive().InsertOneAsync(Arg.Is<Menu>(x => x.Meals.Contains("Foobar")));
			slackbot.Received().SendMessage(string.Empty, $"_Foo_ is already on the menu on *{DateTime.Today:dd.MM.yyyy}*!");
		}
	}
}
