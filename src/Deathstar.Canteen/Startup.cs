using System.Text.Json.Serialization;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Deathstar.Canteen
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class Startup
	{
		private readonly IConfiguration configuration;
		private readonly ILogger logger;

		public Startup(IConfiguration configuration, ILogger<Startup> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		// ReSharper disable once UnusedMember.Global
		public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
		{
			lifetime.ApplicationStarted.Register(() => logger.LogInformation("Application started."));
			lifetime.ApplicationStopping.Register(() => logger.LogInformation("Application stopping.."));
			lifetime.ApplicationStopped.Register(() => logger.LogInformation("Application stopped."));
			app.UseRouting();
			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		// ReSharper disable once UnusedMember.Global
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IMongoClient>(new MongoClient(configuration["connectionString"]));
			services.AddSingleton(
				x =>
				{
					var mongoClient = x.GetService<IMongoClient>();
					IMongoDatabase mongoDatabase = mongoClient.GetDatabase("canteen");
					IMongoCollection<Menu> mongoCollection = mongoDatabase.GetCollection<Menu>("menus");
					mongoCollection.Indexes.CreateOne(new CreateIndexModel<Menu>(Builders<Menu>.IndexKeys.Ascending(y => y.Date)));

					return mongoCollection;
				});
			services.AddSingleton(
				x =>
				{
					var mongoClient = x.GetService<IMongoClient>();
					IMongoDatabase mongoDatabase = mongoClient.GetDatabase("canteen");
					IMongoCollection<ChatResponse> mongoCollection = mongoDatabase.GetCollection<ChatResponse>("chatResponses");
					mongoCollection.Indexes.CreateOne(new CreateIndexModel<ChatResponse>(Builders<ChatResponse>.IndexKeys.Ascending(y => y.Regex)));

					return mongoCollection;
				});
			services.AddTransient<IMenuRepository, MenuRepository>();
			services.AddTransient<IChatResponseRepository, ChatResponseRepository>();

			services.AddSingleton<ISlackbot, Services.Slackbot>();

			services.AddTransient<ICommandDispatcher, CommandDispatcher>();

			services.AddTransient<ICommandHandler, HelpCommandHandler>();
			services.AddTransient<ICommandHandler, AddCommandHandler>();
			services.AddTransient<ICommandHandler, ClearCommandHandler>();
			services.AddTransient<ICommandHandler, DayAfterTomorrowCommandHandler>();
			services.AddTransient<ICommandHandler, TomorrowCommandHandler>();
			services.AddTransient<ICommandHandler, TodayCommandHandler>();
			services.AddTransient<ICommandHandler, ImportCommandHandler>();
			services.AddTransient<ICommandHandler, StatsCommandHandler>();
			services.AddTransient<ICommandHandler, SearchCommandHandler>();
			services.AddTransient<ICommandHandler, NextCommandHandler>();
			services.AddTransient<ICommandHandler, WeekCommandHandler>();
			services.AddTransient<ICommandHandler, ChatCommandHandler>();

			services.AddTransient<IMenuParser, MenuParser>();

			services.AddHostedService<CanteenService>();

			services.AddControllers()
				.AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
		}
	}
}
