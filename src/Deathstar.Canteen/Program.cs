using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Slackbot;

namespace Deathstar.Canteen
{
	internal class Program
	{
		private static Bot Bot { get; set; }

		private static ChatResponse[] ChatResponses { get; set; }

		private static CommandFactory CommandFactory { get; set; }

		private static CommandRequestParser CommandRequestParser { get; set; }

		private static string Username { get; set; }

		private static async Task<bool> Handle( OnMessageArgs message )
		{
			CommandRequest commandRequest = CommandRequestParser.Parse( message );

			if( commandRequest == null )
				return false;

			ICommand command = CommandFactory.GetCommand( commandRequest );

			if( command == null )
				return false;

			string response = await command.HandleAsync();

			if( string.IsNullOrWhiteSpace( response ) )
				return false;

			Bot.SendMessage( message.Channel, response );

			return true;
		}

		private static void Main()
		{
			// Setup application loop
			var exitEvent = new ManualResetEvent( false );
			Console.CancelKeyPress += ( sender, e ) =>
			{
				e.Cancel = true;
				exitEvent.Set();
			};

			// Setup exception handling
			AppDomain.CurrentDomain.UnhandledException += ( sender, args ) =>
			{
				var exception = args.ExceptionObject as Exception;
				Console.Error.WriteLine( $"Error: {exception?.Message ?? "Unknown error occured."}" );
				Console.WriteLine( "Shutting down.." );
				exitEvent.Set();
			};

			// Parse appsettings.json
			IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath( Directory.GetCurrentDirectory() ).AddJsonFile( "appsettings.json" );
			IConfigurationRoot configuration = builder.Build();
			string token = configuration["token"];
			Username = configuration["username"];
			string connectionString = configuration["connectionString"];
			ChatResponses = configuration.GetSection( "chat" ).Get<ChatResponse[]>();

			// Construct dependencies
			Bot = new Bot( token, Username );
			var mongoClient = new MongoClient( connectionString );
			CommandRequestParser = new CommandRequestParser( Username );
			CommandFactory = new CommandFactory( mongoClient );

			// Registering message handler
			Bot.OnMessage += async ( sender, message ) =>
			{
				try
				{
					if( await Handle( message ) )
						return;

					TryToChat( message );
				}
				catch( AggregateException ae )
				{
					ae.Flatten().Handle( e =>
					{
						Console.Error.WriteLine( $"Error: {e.Message}" );
						return true;
					} );
				}
				catch( Exception e )
				{
					Console.Error.WriteLine( $"Error: {e.Message}" );
				}
			};

			// Wait for Ctrl+C
			exitEvent.WaitOne();
		}

		private static void TryToChat( OnMessageArgs message )
		{
			foreach( ChatResponse chatResponse in ChatResponses )
			{
				var chatRequestParser = new ChatRequestParser( Username, chatResponse.Regex );

				if( !chatRequestParser.Parse( message ) )
					continue;

				Bot.SendMessage( message.Channel, chatResponse.Response );

				break;
			}
		}
	}
}
