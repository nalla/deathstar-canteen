using System;
using System.IO;
using Deathstar.Canteen.Persistence;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Deathstar.Canteen.Tests.Helpers
{
	internal static class MongoHelper
	{
		private static Lazy<IMongoClient> LazyClient { get; } = new Lazy<IMongoClient>( () =>
		{
			IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath( Directory.GetCurrentDirectory() ).AddJsonFile( "appsettings.json" );
			IConfigurationRoot configuration = builder.Build();
			return new MongoClient( configuration["connectionString"] );
		} );

		internal static IMongoClient Client => LazyClient.Value;

		internal static IMongoCollection<Menu> Collection => Database.GetCollection<Menu>( "menus" );

		internal static IMongoDatabase Database => Client.GetDatabase( "canteen" );

		internal static void Clear() => Database.DropCollection( "menus" );
	}
}
