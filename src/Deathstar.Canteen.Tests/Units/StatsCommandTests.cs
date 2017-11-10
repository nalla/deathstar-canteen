using System.Threading.Tasks;
using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class StatsCommandTests
	{
		[Fact]
		public async Task TheHandleMethodShouldReturnStats()
		{
			// Arrange
			var command = new StatsCommand( null, MongoHelper.Client );

			// Act
			string response = await command.HandleAsync();

			// Assert
			Assert.Contains( "Private Memory", response );
			Assert.Contains( "Virtual Memory", response );
			Assert.Contains( "Working Memory", response );
			Assert.Contains( "Total Memory", response );
			Assert.Contains( "Starttime", response );
			Assert.Contains( "Uptime", response );
			Assert.Contains( "Saved menus", response );
		}
	}
}
