using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Services
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class MenuRepository : IMenuRepository
	{
		private readonly IMongoCollection<Menu> mongoCollection;

		public MenuRepository(IMongoCollection<Menu> mongoCollection) => this.mongoCollection = mongoCollection;

		public async Task<long> CountAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default) =>
			await mongoCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

		public async Task<long> DeleteOneAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default) =>
			(await mongoCollection.DeleteOneAsync(filter, cancellationToken)).DeletedCount;

		public async Task InsertOneAsync(Menu menu, CancellationToken cancellationToken = default) =>
			await mongoCollection.InsertOneAsync(menu, cancellationToken: cancellationToken);

		public async Task ReplaceOneAsync(Expression<Func<Menu, bool>> filter, Menu replacement, CancellationToken cancellationToken = default) =>
			await mongoCollection.ReplaceOneAsync(filter, replacement, cancellationToken: cancellationToken);

		public async Task ReplaceOrInsertAsync(Menu menu, CancellationToken cancellationToken = default) =>
			await mongoCollection.ReplaceOneAsync(Builders<Menu>.Filter.Eq(x => x.Date, menu.Date), menu, new UpdateOptions { IsUpsert = true }, cancellationToken);

		public async Task<Menu> SingleOrDefaultAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default)
		{
			IAsyncCursor<Menu> cursor = await mongoCollection.FindAsync(filter, cancellationToken: cancellationToken);

			return await cursor.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<List<Menu>> ToListAsync(FilterDefinition<Menu> filter, CancellationToken cancellationToken = default)
		{
			IAsyncCursor<Menu> cursor = await mongoCollection.FindAsync(filter, cancellationToken: cancellationToken);

			return await cursor.ToListAsync(cancellationToken);
		}
	}
}
