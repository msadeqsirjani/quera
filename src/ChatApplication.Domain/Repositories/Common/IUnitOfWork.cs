namespace ChatApplication.Domain.Repositories.Common;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}