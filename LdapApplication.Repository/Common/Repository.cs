using LdapApplication.Model.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LdapApplication.Repository.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbEntitySet;
        private bool _disposed;

        public Repository(DbContext db)
        {
            _context = db;
            _dbEntitySet = db.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return _dbEntitySet.ToList();
        }

        public List<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.ToList();
        }

        public TEntity GetSingle(int id)
        {
            return _dbEntitySet.Find(id);
        }

        public List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbEntitySet.Where(predicate).ToList();
        }

        public void Insert(TEntity entity)
        {
            _context.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var dbEntityEntry = _context.Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _dbEntitySet.Attach(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            _dbEntitySet.Remove(entity);
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbEntitySet.ToListAsync();
        }

        public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.ToListAsync();
        }

        public Task<TEntity> GetSingleAsync(int id)
        {
            return _dbEntitySet.FindAsync(id);
        }

        public Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbEntitySet.Where(predicate).ToListAsync();
        }


        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = _dbEntitySet;
            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
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
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
