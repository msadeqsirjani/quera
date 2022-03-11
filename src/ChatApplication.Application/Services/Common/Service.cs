using System.Linq.Expressions;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services.Common;

public interface IService<T> where T : Entity, new()
{
    IEnumerable<T> Select();
    IEnumerable<T> Select(Expression<Func<T, bool>> predicate);
    T? FirstOrDefault(int id);
    T? FirstOrDefault(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Attach(T entity);
    void AttacheRange(IEnumerable<T> entities);
    void Delete(T entity);
    void Delete(int id);
    void DeleteRange(IEnumerable<int> ids);
    void DeleteRange(Expression<Func<T, bool>> predicate);
    bool Exists(Expression<Func<T, bool>> predicate);
}

public class Service<T> : IService<T> where T : Entity, new()
{
    protected readonly IRepositoryAsync<T> Repository;

    public Service(IRepositoryAsync<T> repository)
    {
        Repository = repository;
    }

    public virtual IEnumerable<T> Select() => Repository.Queryable(false);

    public virtual IEnumerable<T> Select(Expression<Func<T, bool>> predicate) =>
        Repository.Queryable(predicate, false);

    public virtual T? FirstOrDefault(int id) => Repository.FirstOrDefault(x => x.Id == id);

    public virtual T? FirstOrDefault(Expression<Func<T, bool>> predicate) => Repository.FirstOrDefault(predicate);

    public virtual void Add(T entity) => Repository.Add(entity);

    public virtual void AddRange(IEnumerable<T> entities) => Repository.AddRange(entities);

    public virtual void Update(T entity) => Repository.Update(entity);

    public virtual void UpdateRange(IEnumerable<T> entities) => Repository.UpdateRange(entities);

    public virtual void Attach(T entity) => Repository.Attach(entity);

    public virtual void AttacheRange(IEnumerable<T> entities) => Repository.AttacheRange(entities);

    public virtual void Delete(T entity) => Repository.Delete(entity);

    public virtual void Delete(int id) => Repository.Delete(id);

    public virtual void DeleteRange(IEnumerable<int> ids) => Repository.DeleteRange(ids);

    public virtual void DeleteRange(Expression<Func<T, bool>> predicate) => Repository.DeleteRange(predicate);

    public virtual bool Exists(Expression<Func<T, bool>> predicate) => Repository.Exists(predicate);
}