namespace Deathstar.Canteen.Commands.Abstractions
{
	public interface ICommandMessage
	{
		string Arguments { get; }

		string Channel { get; }

		string Name { get; }
	}
}
