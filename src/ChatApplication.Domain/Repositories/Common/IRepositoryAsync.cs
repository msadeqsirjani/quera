using System.Linq.Expressions;
using ChatApplication.Domain.Common;
using Microsoft.Data.SqlClient;

namespace ChatApplication.Domain.Repositories.Common;

public interface IRepositoryAsync<T> : IAsyncDisposable, IRepository<T> where T : Entity, new()
{
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> ExecuteScalarAsync(string query, CancellationToken cancellationToken = default, params SqlParameter[] parameters);
    Task ExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default, params SqlParameter[] parameters);
    Task<IEnumerable<T>> ExecuteReaderAsync(string query, CancellationToken cancellationToken = default, params SqlParameter[] parameters);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}