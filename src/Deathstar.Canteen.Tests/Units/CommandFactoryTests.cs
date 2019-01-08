using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Tests.Mocks;
using Xunit;

namespace Deathstar.Canteen.Tests.Units
{
	public class CommandFactoryTests
	{
		private readonly ICommandFactory commandFactory;

		public CommandFactoryTests() => commandFactory = new CommandFactory(new[] { new FakeCommand() });

		[Fact]
		public void TheCommandFactoryShouldConstructNothingWhenNullIsProvidedAsCommandName()
		{
			// Act
			ICommand command = commandFactory.GetCommand(null);

			// Assert
			Assert.Null(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructNothingWhenUnknownCommandNameIsProvided()
		{
			// Act
			ICommand command = commandFactory.GetCommand("unknown");

			// Assert
			Assert.Null(command);
		}

		[Fact]
		public void TheCommandFactoryShouldConstructCommandWhenCommandNameIsProvided()
		{
			// Act
			ICommand command = commandFactory.GetCommand("fake");

			// Assert
			Assert.IsType<FakeCommand>(command);
		}

		[Theory]
		[InlineData("fake")]
		[InlineData("FAKE")]
		[InlineData("fAKe")]
		public void TheCommandFactoryShouldNotCareAboutCaseSensitivity(string name)
		{
			// Act
			ICommand command = commandFactory.GetCommand(name);

			// Assert
			Assert.IsType<FakeCommand>(command);
		}
	}
}

