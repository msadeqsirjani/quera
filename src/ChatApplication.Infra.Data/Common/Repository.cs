using System.Data;
using System.Linq.Expressions;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Repositories.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Infra.Data.Common;

public class Repository<T> : IRepository<T> where T : Entity, new()
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public IQueryable<T> Queryable(bool enableTracking = true)
    {
        return !enableTracking ? DbSet.AsNoTracking() : DbSet;
    }

    public IQueryable<T> Queryable(Expression<Func<T, bool>> predicate, bool enableTracking = true)
    {
        return !enableTracking ? DbSet.AsNoTracking().Where(predicate) : DbSet.Where(predicate);
    }

    public T? FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return DbSet.FirstOrDefault(predicate);
    }

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public void Attach(T entity)
    {
        DbSet.Attach(entity);
    }

    public void AttacheRange(IEnumerable<T> entities)
    {
        DbSet.AttachRange(entities);
    }

    public void Delete(int id)
    {
        var entity = FirstOrDefault(x => x.Id == id);

        if (entity == null) return;

        DbSet.Remove(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<int> ids)
    {
        DeleteRange(x => ids.Contains(x.Id));
    }

    public void DeleteRange(Expression<Func<T, bool>> predicate)
    {
        var entities = Queryable(predicate).ToList();

        DbSet.RemoveRange(entities);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return DbSet.Any(predicate);
    }

    public int Count(Expression<Func<T, bool>> predicate)
    {
        return DbSet.Count(predicate);
    }

    public T? ExecuteScalar(string query, params SqlParameter[] parameters)
    {
        var connection = Context.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter);
        if (connection.State.Equals(ConnectionState.Closed))
            connection.Open();
        return command.ExecuteScalar() as T;
    }

    public void ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        var connection = Context.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter);
        if (connection.State.Equals(ConnectionState.Closed))
            connection.Open();
        command.ExecuteNonQuery();
    }

    public IEnumerable<T> ExecuteReader(string query, params SqlParameter[] parameters)
    {
        var connection = Context.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter);
        if (connection.State.Equals(ConnectionState.Closed))
            connection.Open();
        var values = command.ExecuteReader();

        List<T> entities = new();

        while (values.Read())
        {
            T entity = new();

            for (var i = 0; i < values.FieldCount; i++)
            {
                var type = entity.GetType();
                var property = type.GetProperty(values.GetName(i));
                property?.SetValue(entity, values.GetValue(i), null);
            }

            entities.Add(entity);
        }

        return entities;
    }

    public void SaveChanges()
    {
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}