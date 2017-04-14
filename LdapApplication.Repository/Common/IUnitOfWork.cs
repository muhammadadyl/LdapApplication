using System.Threading;
using System.Threading.Tasks;
using LdapApplication.Model.Common;
using System;

namespace LdapApplication.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Dispose(bool disposing);
    }
}