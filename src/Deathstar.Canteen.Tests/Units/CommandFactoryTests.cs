using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandFactoryTests
	{
		[Fact]
		public void TheCommandFactoryShouldConstrucAddCommandWhenAddCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new AddCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("add");

			// Assert
			Assert.IsType<AddCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucClearCommandWhenClearCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new ClearCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("clear");

			// Assert
			Assert.IsType<ClearCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucImportCommandWhenImportCommandNameIsProvided()
		{
			// Arrange
			var logger = Substitute.For<ILogger<ImportCommand>>();
			var factory = new CommandFactory(new ICommand[] { new ImportCommand(null, null, logger) });

			// Act
			ICommand command = factory.GetCommand("import");

			// Assert
			Assert.IsType<ImportCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucNextCommandWhenNextCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new NextCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("next");

			// Assert
			Assert.IsType<NextCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucSearchCommandWhenSearchCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new SearchCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("search");

			// Assert
			Assert.IsType<SearchCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructDayAfterTomorrowCommandWhenDayAfterTomorrowCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new DayAfterTomorrowCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("dayaftertomorrow");

			// Assert
			Assert.IsType<DayAfterTomorrowCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructNothingWhenNullIsProvidedAsCommandName()
		{
			// Arrange
			var factory = new CommandFactory(null);

			// Act
			ICommand command = factory.GetCommand(null);

			// Assert
			Assert.Null(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructNothingWhenUnknownCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(null);

			// Act
			ICommand command = factory.GetCommand("unknown");

			// Assert
			Assert.Null(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructStatsCommandWhenStatsCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new StatsCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("stats");

			// Assert
			Assert.IsType<StatsCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructTodayCommandWhenTodayCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new TodayCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("today");

			// Assert
			Assert.IsType<TodayCommand>(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructTomorrowCommandWhenTomorrowCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new TomorrowCommand(null, null) });

			// Act
			ICommand command = factory.GetCommand("tomorrow");

			// Assert
			Assert.IsType<TomorrowCommand>(command);
		}

		[Theory]
		[InlineData("help")]
		[InlineData("HELP")]
		[InlineData("hELp")]
		public void TheCommandFactoryShouldNotCareAboutCaseSensitivity(string name)
		{
			// Arrange
			var factory = new CommandFactory(new ICommand[] { new HelpCommand(null) });

			// Act
			ICommand command = factory.GetCommand(name);

			// Assert
			Assert.IsType<HelpCommand>(command);
		}
	}
}
