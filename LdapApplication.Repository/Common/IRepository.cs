using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LdapApplication.Model.Common;

namespace LdapApplication.Repository.Common
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        List<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetSingle(int id);
        Task<TEntity> GetSingleAsync(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Dispose();
        void Dispose(bool disposing);
    }
}