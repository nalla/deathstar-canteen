using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Deathstar.Canteen.Persistence;
using MongoDB.Driver;

namespace Deathstar.Canteen.Services
{
	public interface IMenuRepository
	{
		Task<long> CountAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default);

		Task<long> DeleteOneAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default);

		Task InsertOneAsync(Menu menu, CancellationToken cancellationToken = default);

		Task ReplaceOneAsync(Expression<Func<Menu, bool>> filter, Menu replacement, CancellationToken cancellationToken = default);

		Task<Menu> SingleOrDefaultAsync(Expression<Func<Menu, bool>> filter, CancellationToken cancellationToken = default);

		Task<List<Menu>> ToListAsync(FilterDefinition<Menu> filter, CancellationToken cancellationToken = default);
	}
}
