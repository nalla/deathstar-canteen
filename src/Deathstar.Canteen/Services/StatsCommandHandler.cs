using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deathstar.Canteen.Services
{
	public class StatsCommandHandler : ICommandHandler
	{
		private readonly IMenuRepository menuRepository;
		private readonly ISlackbot slackbot;

		public StatsCommandHandler(IMenuRepository menuRepository, ISlackbot slackbot)
		{
			this.menuRepository = menuRepository;
			this.slackbot = slackbot;
		}

		public string HelpText { get; } = "The *stats* command will display internal statistics of the canteen.";

		public string SupportedCommandName { get; } = "stats";

		public async Task HandleAsync(string arguments, string channel, CancellationToken cancellationToken)
		{
			var sb = new StringBuilder();
			Process process = Process.GetCurrentProcess();
			TimeSpan uptime = DateTime.Now - process.StartTime;

			sb.AppendLine("*AspNetCore Runtime*");
			sb.AppendLine($"Version: {GetNetCoreVersion()}");
			sb.AppendLine();
			sb.AppendLine("*Service*");
			sb.AppendLine($"Private Memory: {process.PrivateMemorySize64 / 1024 / 1024} MB");
			sb.AppendLine($"Virtual Memory: {process.VirtualMemorySize64 / 1024 / 1024} MB");
			sb.AppendLine($"Working Memory: {process.WorkingSet64 / 1024 / 1024} MB");
			sb.AppendLine($"Total Memory: {GC.GetTotalMemory(false) / 1024 / 1024} MB");
			sb.AppendLine($"Starttime: {process.StartTime:u}");
			sb.AppendLine($"Uptime: {uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds");
			sb.AppendLine();
			sb.AppendLine("*Database*");
			sb.AppendLine($"Saved menus: {await menuRepository.CountAsync(_ => true, cancellationToken)}");
			sb.AppendLine();
			sb.AppendLine("*Contribution*");
			sb.AppendLine("https://github.com/nalla/deathstar-canteen");

			slackbot.SendMessage(channel, sb.ToString());
		}

		private static string GetNetCoreVersion()
		{
			var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
			var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");

			return netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2 ? assemblyPath[netCoreAppIndex + 1] : null;
		}
	}
}
