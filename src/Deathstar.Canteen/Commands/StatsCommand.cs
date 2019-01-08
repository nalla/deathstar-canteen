using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Slack;

namespace Deathstar.Canteen.Commands
{
	public class StatsCommand : ICommand
	{
		private readonly IMenuCollection menuCollection;
		private readonly ISlackbot slackbot;

		public StatsCommand(IMenuCollection menuCollection, ISlackbot slackbot)
		{
			this.menuCollection = menuCollection;
			this.slackbot = slackbot;
		}

		public async Task HandleAsync(ICommandMessage message, CancellationToken cancellationToken)
		{
			var sb = new StringBuilder();
			Process process = Process.GetCurrentProcess();
			TimeSpan uptime = DateTime.Now - process.StartTime;

			sb.AppendLine("*Runtime*");
			sb.AppendLine($"Private Memory: {process.PrivateMemorySize64 / 1024 / 1024} MB");
			sb.AppendLine($"Virtual Memory: {process.VirtualMemorySize64 / 1024 / 1024} MB");
			sb.AppendLine($"Working Memory: {process.WorkingSet64 / 1024 / 1024} MB");
			sb.AppendLine($"Total Memory: {GC.GetTotalMemory(false) / 1024 / 1024} MB");
			sb.AppendLine($"Starttime: {process.StartTime:u}");
			sb.AppendLine($"Uptime: {uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds");
			sb.AppendLine();
			sb.AppendLine("*Database*");
			sb.AppendLine($"Saved menus: {await menuCollection.CountAsync(_ => true, cancellationToken)}");
			sb.AppendLine();
			sb.AppendLine("Code: https://github.com/nalla/deathstar-canteen");

			slackbot.SendMessage(message.Channel, sb.ToString());
		}
	}
}
