using LdapApplication.Model.Common;
using LdapApplication.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LdapApplication.Services.Common
{
    public class Service<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        protected readonly IRepository<TEntity> _repository;
        private bool _disposed;

        public Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _repository = UnitOfWork.Repository<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public TEntity GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(TEntity entity)
        {
            _repository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            return _repository.GetSingleAsync(id);
        }

        public Task AddAsync(TEntity entity)
        {
            _repository.Insert(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            _repository.Delete(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                UnitOfWork.Dispose();
            }
            _disposed = true;
        }
    }
}
