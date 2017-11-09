using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Deathstar.Canteen.Commands.Abstractions;
using Deathstar.Canteen.Persistence;
using Flurl;
using Flurl.Http;
using MongoDB.Driver;

namespace Deathstar.Canteen.Commands
{
	public class ImportCommand : Command
	{
		private Regex Regex { get; } = new Regex( "^\\d{8}$", RegexOptions.Compiled );

		public ImportCommand( string arguments, IMongoClient mongoClient ) : base( arguments, mongoClient ) { }

		public override async Task<string> HandleAsync()
		{
			Log( $"Trying to import from url {Arguments}" );

			var url = new Url( ( Arguments ?? "" ).TrimStart( '<' ).TrimEnd( '>' ) );

			if( !url.IsValid() )
				return "You need to provide a well formed url.";

			Menu[] menus;

			try
			{
				menus = await url.GetJsonAsync<Menu[]>();
			}
			catch( FlurlHttpException )
			{
				return "I got an error when downloading the url you provided.";
			}

			int i = 0;

			foreach( Menu menu in menus )
			{
				if( Regex.IsMatch( menu.Date ?? "" ) &&
					menu.Meals?.Length > 0 &&
					menu.Meals.All( x => !string.IsNullOrWhiteSpace( x ) ) &&
					await MongoCollection.CountAsync( x => x.Date == menu.Date ) == 0 )
				{
					await MongoCollection.InsertOneAsync( menu );
					i++;
				}
			}

			return $"I imported {i} menus.";
		}
	}
}
