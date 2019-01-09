using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;
using Deathstar.Canteen.Tests.Mocks;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class ImportCommandTests
	{
		private readonly IMenuCollection menuCollection = Substitute.For<IMenuCollection>();
		private readonly ISlackbot slackbot = Substitute.For<ISlackbot>();
		private readonly ILogger<ImportCommand> logger = Substitute.For<ILogger<ImportCommand>>();
		private readonly ICommand command;

		public ImportCommandTests() => command = new ImportCommand(menuCollection, slackbot, logger);

		[Fact]
		public async Task TheHandleMethodShouldAcceptWellFormedUrlsFromSlackAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[] { new { date = "20010101", meals = new[] { "foo", "bar" } } };
				var commandMessage = new FakeCommandMessage("<https://api.myjson.com/bins/1dekrb>", string.Empty);

				httpTest.RespondWithJson(importData);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("https://api.myjson.com/bins/1dekrb");
				slackbot.Received().SendMessage(string.Empty, "I imported 1 menus.");
				await menuCollection.Received().InsertOneAsync(
					Arg.Is<Menu>(
						x =>
							x.Date == "20010101" &&
							x.Meals[0] == "foo" &&
							x.Meals[1] == "bar"),
					CancellationToken.None);
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldCheckDateFormatAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[] { new { date = "foo", meals = new[] { "foo" } } };
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWithJson(importData);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I imported 0 menus.");
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldCheckThatMealsAreValidAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[] { new { date = "20010101", meals = new[] { "foo", " " } } };
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWithJson(importData);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I imported 0 menus.");
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldCheckThatMealsExistsAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[] { new { date = "20010101", meals = new[] { string.Empty } } };
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWithJson(importData);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I imported 0 menus.");
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldInsertMenuIntoDatabaseAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[] { new { date = "20010101", meals = new[] { "foo", "bar" } } };
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWithJson(importData);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I imported 1 menus.");
				await menuCollection.Received().InsertOneAsync(
					Arg.Is<Menu>(
						x =>
							x.Date == "20010101" &&
							x.Meals[0] == "foo" &&
							x.Meals[1] == "bar"),
					CancellationToken.None);
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldInsertMenuOnlyOnceIntoDatabaseAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", "bar" } },
					new { date = "20010101", meals = new[] { "bar", "foo" } },
				};
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWithJson(importData);
				menuCollection.CountAsync(Arg.Any<Expression<Func<Menu, bool>>>(), CancellationToken.None).Returns(0, 1);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I imported 1 menus.");
				await menuCollection.Received().InsertOneAsync(
					Arg.Is<Menu>(
						x =>
							x.Date == "20010101" &&
							x.Meals[0] == "foo" &&
							x.Meals[1] == "bar"),
					CancellationToken.None);
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldInsertMultipleMenusIntoDatabaseAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var importData = new[]
				{
					new { date = "20010101", meals = new[] { "foo", "bar" } },
					new { date = "20010102", meals = new[] { "bar", "foo" } },
				};
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWithJson(importData);

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I imported 2 menus.");
				await menuCollection.Received().InsertOneAsync(
					Arg.Is<Menu>(
						x =>
							x.Date == "20010101" &&
							x.Meals[0] == "foo" &&
							x.Meals[1] == "bar"),
					CancellationToken.None);
				await menuCollection.Received().InsertOneAsync(
					Arg.Is<Menu>(
						x =>
							x.Date == "20010102" &&
							x.Meals[0] == "bar" &&
							x.Meals[1] == "foo"),
					CancellationToken.None);
			}
		}

		[Fact]
		public async Task TheHandleMethodShouldReturnNoticeAboutDownloadErrorAsync()
		{
			using (var httpTest = new HttpTest())
			{
				// Arrange
				var commandMessage = new FakeCommandMessage("http://localhost/foobar", string.Empty);

				httpTest.RespondWith();

				// Act
				await command.HandleAsync(commandMessage, CancellationToken.None);

				// Assert
				httpTest.ShouldHaveCalled("http://localhost/foobar");
				slackbot.Received().SendMessage(string.Empty, "I got an error while downloading the url you provided.");
			}
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("foobar")]
		[InlineData("1234")]
		[InlineData(":/foo/bar")]
		public async Task TheHandleMethodShouldReturnNoticeAboutInvalidUrlAsync(string arguments)
		{
			// Arrange
			var commandMessage = new FakeCommandMessage(arguments, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(string.Empty, "You need to provide a well formed url.");
		}
	}
}
