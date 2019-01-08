using Deathstar.Canteen.Commands;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Deathstar.Canteen.Services;
using Deathstar.Canteen.Slack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
		public void Configure(IApplicationLifetime lifetime)
		{
			lifetime.ApplicationStarted.Register(() => logger.LogInformation("Application started."));
			lifetime.ApplicationStopping.Register(() => logger.LogInformation("Application stopping.."));
			lifetime.ApplicationStopped.Register(() => logger.LogInformation("Application stopped."));
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
			services.AddTransient<IMenuCollection, MenuCollection>();
			services.AddTransient<IChatResponseCollection, ChatResponseCollection>();

			services.AddSingleton<ISlackbot, Slack.Slackbot>();

			services.AddTransient<ICommandMessageParser, CommandMessageParser>();
			services.AddTransient<ICommandFactory, CommandFactory>();

			services.AddTransient<ICommand, HelpCommand>();
			services.AddTransient<ICommand, AddCommand>();
			services.AddTransient<ICommand, ClearCommand>();
			services.AddTransient<ICommand, DayAfterTomorrowCommand>();
			services.AddTransient<ICommand, TomorrowCommand>();
			services.AddTransient<ICommand, TodayCommand>();
			services.AddTransient<ICommand, ImportCommand>();
			services.AddTransient<ICommand, StatsCommand>();
			services.AddTransient<ICommand, SearchCommand>();
			services.AddTransient<ICommand, NextCommand>();
			services.AddTransient<ICommand, ChatCommand>();

			services.AddHostedService<CanteenService>();
		}
	}
}
