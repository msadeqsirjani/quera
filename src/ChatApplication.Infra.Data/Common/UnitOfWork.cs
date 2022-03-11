using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Infra.Data.Common;

public class UnitOfWork : IUnitOfWork
{
    protected readonly ApplicationDbContext Context;

    public UnitOfWork(ApplicationDbContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        Context.Dispose();
    }

    public void SaveChanges()
    {
        Context.SaveChanges();
    }

    public void BeginTransaction()
    {
        Context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Context.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Context.Database.RollbackTransaction();
    }
}