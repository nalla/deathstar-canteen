using System.Threading.Tasks;
using Deathstar.Canteen.Services;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class StatsCommandHandlerTests
	{
		[Fact]
		public async Task TheHandleMethodShouldReturnStatsAsync()
		{
			// Arrange
			var menuCollection = Substitute.For<IMenuRepository>();
			var slackbot = Substitute.For<ISlackbot>();
			ICommandHandler commandHandler = new StatsCommandHandler(menuCollection, slackbot);

			// Act
			await commandHandler.HandleAsync(null, string.Empty, default);

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
