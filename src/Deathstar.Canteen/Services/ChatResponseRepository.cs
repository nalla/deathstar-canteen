using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Services
{
	public class ChatResponseRepository : IChatResponseRepository
	{
		private readonly IMongoCollection<ChatResponse> mongoCollection;

		public ChatResponseRepository(IMongoCollection<ChatResponse> mongoCollection) => this.mongoCollection = mongoCollection;

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

		public async Task<bool> RemoveAsync(string regex) => (await mongoCollection.DeleteOneAsync(x => x.Regex == regex)).DeletedCount == 1;
	}
}
