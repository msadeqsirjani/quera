using System.Linq.Expressions;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services.Common;

public interface IServiceAsync<T> : IService<T> where T : Entity, new()
{
    Task<T?> FirstOrDefaultAsync(int id, CancellationToken cancellationToken = new());
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new());
    Task AddAsync(T entity, CancellationToken cancellationToken = new());
    Task AddRangeAsync(IEnumerable<T> entities);
    Task DeleteAsync(int id, CancellationToken cancellationToken = new());
    Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = new());
    Task DeleteRangeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new());
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new());
}

public class ServiceAsync<T> : Service<T>, IServiceAsync<T> where T : Entity, new()
{
    public ServiceAsync(IRepositoryAsync<T> repository) : base(repository)
    {
    }

    public virtual Task<T?> FirstOrDefaultAsync(int id, CancellationToken cancellationToken = new()) => Repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public virtual Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new()) =>
        Repository.FirstOrDefaultAsync(predicate, cancellationToken);

    public virtual Task AddAsync(T entity, CancellationToken cancellationToken = new()) => Repository.AddAsync(entity, cancellationToken);

    public virtual Task AddRangeAsync(IEnumerable<T> entities) => Repository.AddRangAsync(entities);

    public virtual Task DeleteAsync(int id, CancellationToken cancellationToken = new()) =>
        Repository.DeleteAsync(id, cancellationToken);

    public virtual Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = new()) =>
        Repository.DeleteRangeAsync(ids, cancellationToken);

    public virtual Task DeleteRangeAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = new()) => Repository.DeleteRangeAsync(predicate, cancellationToken);

    public virtual Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = new()) => Repository.ExistsAsync(predicate, cancellationToken);
}