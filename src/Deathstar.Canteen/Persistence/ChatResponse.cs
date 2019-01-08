using MongoDB.Bson;

namespace Deathstar.Canteen.Persistence
{
	public class ChatResponse
	{
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public ObjectId Id { get; set; }

		public string Regex { get; set; }

		public string Response { get; set; }
	}
}
