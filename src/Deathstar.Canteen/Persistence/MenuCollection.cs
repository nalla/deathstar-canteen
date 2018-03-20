using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Deathstar.Canteen.Persistence
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class MenuCollection : IMenuCollection
	{
		public MenuCollection(IMongoCollection<Menu> mongoCollection) => MongoCollection = mongoCollection;

		private IMongoCollection<Menu> MongoCollection { get; }

		public async Task<long> CountAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default) =>
			await MongoCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

		public async Task<long> DeleteOneAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default) =>
			(await MongoCollection.DeleteOneAsync(filter, cancellationToken)).DeletedCount;

		public async Task InsertOneAsync(Menu menu, CancellationToken cancellationToken = default) =>
			await MongoCollection.InsertOneAsync(menu, cancellationToken: cancellationToken);

		public async Task ReplaceOneAsync(Expression<Func<Menu, bool>> filter, Menu replacement, CancellationToken cancellationToken = default) =>
			await MongoCollection.ReplaceOneAsync(filter, replacement, cancellationToken: cancellationToken);

		public async Task<Menu> SingleOrDefaultAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default)
		{
			IAsyncCursor<Menu> cursor = await MongoCollection.FindAsync(filter, cancellationToken: cancellationToken);

			return await cursor.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<List<Menu>> ToListAsync(FilterDefinition<Menu> filter, CancellationToken cancellationToken = default)
		{
			IAsyncCursor<Menu> cursor = await MongoCollection.FindAsync(filter, cancellationToken: cancellationToken);

			return await cursor.ToListAsync(cancellationToken);
		}
	}
}
