using System.Data;
using System.Linq.Expressions;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Repositories.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Infra.Data.Common;

public class RepositoryAsync<T> : Repository<T>, IRepositoryAsync<T> where T : Entity, new()
{
    public RepositoryAsync(ApplicationDbContext context) : base(context)
    {
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.AddAsync(entity, cancellationToken);

        return Task.CompletedTask;
    }

    public Task AddRangAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbSet.AddRangeAsync(entities, cancellationToken);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null) return;

        DbSet.Remove(entity);
    }

    public Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return DeleteRangeAsync(x => ids.Contains(x.Id), cancellationToken);
    }

    public Task DeleteRangeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entities = Queryable(predicate).ToList();

        DbSet.RemoveRange(entities);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(predicate, cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return DbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task<T?> ExecuteScalarAsync(string query, CancellationToken cancellationToken = default,
        params SqlParameter[] parameters)
    {
        var connection = Context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter);
        if (connection.State.Equals(ConnectionState.Closed))
            await connection.OpenAsync(cancellationToken);
        return command.ExecuteScalarAsync(cancellationToken) as T;
    }

    public Task ExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default,
        params SqlParameter[] parameters)
    {
        var connection = Context.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter);
        if (connection.State.Equals(ConnectionState.Closed))
            connection.OpenAsync(cancellationToken);
        return command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> ExecuteReaderAsync(string query, CancellationToken cancellationToken = default,
        params SqlParameter[] parameters)
    {
        var connection = Context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter);
        if (connection.State.Equals(ConnectionState.Closed))
            await connection.OpenAsync(cancellationToken);
        var values = await command.ExecuteReaderAsync(cancellationToken);

        List<T> entities = new();

        while (await values.ReadAsync(cancellationToken))
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

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return Context.DisposeAsync();
    }
}