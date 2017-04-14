using LdapApplication.Model.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LdapApplication.Services.Common
{
    public interface IService<TEntity> : IGenericService where TEntity : BaseEntity
    {
        List<TEntity> GetAll();
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}