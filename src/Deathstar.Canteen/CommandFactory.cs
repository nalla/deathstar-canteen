using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen
{
	public class CommandFactory
	{
		private IMongoClient MongoClient { get; }

		public CommandFactory( IMongoClient mongoClient )
		{
			MongoClient = mongoClient;
		}

		public ICommand GetCommand( CommandRequest commandRequest )
		{
			switch( commandRequest?.Name?.ToLower() )
			{
				case "help":
					return new HelpCommand( commandRequest.Arguments, MongoClient );

				case "today":
					return new TodayCommand( commandRequest.Arguments, MongoClient );

				case "tomorrow":
					return new TomorrowCommand( commandRequest.Arguments, MongoClient );

				case "dayaftertomorrow":
					return new DayAfterTomorrowCommand( commandRequest.Arguments, MongoClient );

				case "next":
					return new NextCommand( commandRequest.Arguments, MongoClient );

				case "add":
					return new AddCommand( commandRequest.Arguments, MongoClient );

				case "clear":
					return new ClearCommand( commandRequest.Arguments, MongoClient );

				case "import":
					return new ImportCommand( commandRequest.Arguments, MongoClient );

				default:
					return null;
			}
		}
	}
}
