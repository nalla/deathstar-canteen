using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Deathstar.Canteen
{
	internal static class Program
	{
		private static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			IConfigurationRoot hostConfig = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("hosting.json", true)
				.Build();

			return WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(hostConfig)
				.UseStartup<Startup>()
				.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
		}

		private static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();
	}
}
