using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Tests.Helpers;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandFactoryTests
	{
		[Theory]
		[InlineData( "help" )]
		[InlineData( "HELP" )]
		[InlineData( "hELp" )]
		public void TheCommandFactoryShouldNotCareAboutCaseSensitivity( string name )
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( name ) );

			// Assert
			Assert.IsType<HelpCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucAddCommandWhenAddCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "add" ) );

			// Assert
			Assert.IsType<AddCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucClearCommandWhenClearCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "clear" ) );

			// Assert
			Assert.IsType<ClearCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucImportCommandWhenImportCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "import" ) );

			// Assert
			Assert.IsType<ImportCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucNextCommandWhenNextCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "next" ) );

			// Assert
			Assert.IsType<NextCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstrucSearchCommandWhenSearchCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "search" ) );

			// Assert
			Assert.IsType<SearchCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructDayAfterTomorrowCommandWhenDayAfterTomorrowCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "dayaftertomorrow" ) );

			// Assert
			Assert.IsType<DayAfterTomorrowCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructHelpCommandWhenHelpCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "help" ) );

			// Assert
			Assert.IsType<HelpCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructNothingWhenNullIsProvidedAsCommandName()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( null ) );

			// Assert
			Assert.Null( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructNothingWhenUnknownCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "Unknown" ) );

			// Assert
			Assert.Null( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructStatsCommandWhenStatsCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "stats" ) );

			// Assert
			Assert.IsType<StatsCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructTodayCommandWhenTodayCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "today" ) );

			// Assert
			Assert.IsType<TodayCommand>( command );
		}

		[Fact]
		public void TheCommandFactoryShouldConstructTomorrowCommandWhenTomorrowCommandNameIsProvided()
		{
			// Arrange
			var factory = new CommandFactory( MongoHelper.Client );

			// Act
			ICommand command = factory.GetCommand( new CommandRequest( "tomorrow" ) );

			// Assert
			Assert.IsType<TomorrowCommand>( command );
		}
	}
}
