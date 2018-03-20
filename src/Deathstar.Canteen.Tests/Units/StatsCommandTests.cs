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
	public class StatsCommandTests
	{
		[Fact]
		public async Task TheHandleMethodShouldReturnStatsAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuCollection>();
			var slackbot = Substitute.For<ISlackbot>();
			var command = new StatsCommand(menuCollection, slackbot);
			var commandMessage = new FakeCommandMessage(null, string.Empty);

			// Act
			await command.HandleAsync(commandMessage, CancellationToken.None);

			// Assert
			slackbot.Received().SendMessage(
				string.Empty,
				Arg.Is<string>(
					x =>
						x.Contains("Private Memory") &&
						x.Contains("Virtual Memory") &&
						x.Contains("Working Memory") &&
						x.Contains("Total Memory") &&
						x.Contains("Starttime") &&
						x.Contains("Uptime") &&
						x.Contains("Saved menus")));
		}
	}
}
