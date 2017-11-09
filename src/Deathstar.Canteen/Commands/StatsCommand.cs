using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class StatsCommand : Command
	{
		public StatsCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override async Task<string> HandleAsync()
		{
			var sb = new StringBuilder();
			Process process = Process.GetCurrentProcess();
			TimeSpan uptime = DateTime.Now - process.StartTime;

			sb.AppendLine( "*Runtime*" );
			sb.AppendLine( $"Private Memory: {process.PrivateMemorySize64 / 1024 / 1024} MB" );
			sb.AppendLine( $"Virtual Memory: {process.VirtualMemorySize64 / 1024 / 1024} MB" );
			sb.AppendLine( $"Working Memory: {process.WorkingSet64 / 1024 / 1024} MB" );
			sb.AppendLine( $"Total Memory: {GC.GetTotalMemory( false ) / 1024 / 1024} MB" );
			sb.AppendLine( $"Starttime: {process.StartTime:u}" );
			sb.AppendLine( $"Uptime: {uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds" );
			sb.AppendLine();
			sb.AppendLine( "*Database*" );
			sb.AppendLine( $"Saved menus: {await MongoCollection.CountAsync( _ => true )}" );

			return sb.ToString();
		}
	}
}
