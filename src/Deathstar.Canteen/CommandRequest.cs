namespace Deathstar.Canteen {
	public class CommandRequest
	{
		public CommandRequest( string name, string arguments = "" )
		{
			Name = name;
			Arguments = arguments;
		}

		public string Arguments { get; }

		public string Name { get; }
	}
}
