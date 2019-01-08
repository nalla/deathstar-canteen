using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Deathstar.Canteen.Persistence
{
	public class ChatResponseCollection : IChatResponseCollection
	{
		private readonly IMongoCollection<ChatResponse> mongoCollection;

		public ChatResponseCollection(IMongoCollection<ChatResponse> mongoCollection) => this.mongoCollection = mongoCollection;

		public async Task AddAsync(string regex, string response) =>
			await mongoCollection.UpdateOneAsync(
				Builders<ChatResponse>.Filter.Eq(x => x.Regex, regex),
				Builders<ChatResponse>.Update.Set(x => x.Response, response),
				new UpdateOptions
				{
					IsUpsert = true,
				});

		public async Task<IEnumerable<ChatResponse>> GetAsync(CancellationToken cancellationToken) =>
			(await mongoCollection.FindAsync(FilterDefinition<ChatResponse>.Empty, cancellationToken: cancellationToken)).ToEnumerable();

		public async Task RemoveAsync(string regex) => await mongoCollection.DeleteOneAsync(x => x.Regex == regex);
	}
}
