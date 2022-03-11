using System.Linq.Expressions;
using ChatApplication.Domain.Common;
using Microsoft.Data.SqlClient;

namespace ChatApplication.Domain.Repositories.Common;

public interface IRepository<T> : IDisposable where T : Entity, new()
{
    IQueryable<T> Queryable(bool enableTracking = true);
    IQueryable<T> Queryable(Expression<Func<T, bool>> predicate, bool enableTracking = true);
    T? FirstOrDefault(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Attach(T entity);
    void AttacheRange(IEnumerable<T> entities);
    void Delete(int id);
    void Delete(T entity);
    void DeleteRange(IEnumerable<int> ids);
    void DeleteRange(Expression<Func<T, bool>> predicate);
    bool Exists(Expression<Func<T, bool>> predicate);
    int Count(Expression<Func<T, bool>> predicate);
    T? ExecuteScalar(string query, params SqlParameter[] parameters);
    void ExecuteNonQuery(string query, params SqlParameter[] parameters);
    IEnumerable<T> ExecuteReader(string query, params SqlParameter[] parameters);
    void SaveChanges();
}