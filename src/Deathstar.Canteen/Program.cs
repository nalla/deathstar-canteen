using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Slackbot;

namespace Deathstar.Canteen
{
	internal class Program
	{
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
			string username = configuration["username"];
			string connectionString = configuration["connectionString"];

			// Construct dependencies
			var bot = new Bot( token, username );
			var mongoClient = new MongoClient( connectionString );
			var commandRequestParser = new CommandRequestParser( username );
			var commandFactory = new CommandFactory( mongoClient );

			Console.WriteLine( "Opening canteen.." );
			Console.WriteLine( "Press Ctrl+C to shut down the canteen." );

			// Registering message handler
			bot.OnMessage += ( sender, message ) =>
			{
				CommandRequest commandRequest = commandRequestParser.Parse( message );

				if( commandRequest == null )
					return;

				string response = commandFactory.GetCommand( commandRequest )?.Handle();

				if( !string.IsNullOrWhiteSpace( response ) )
					bot.SendMessage( message.Channel, response );
			};

			// Wait for Ctrl+C
			exitEvent.WaitOne();
		}
	}
}
