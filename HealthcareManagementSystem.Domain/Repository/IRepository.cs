using HealthcareManagementSystem.Domain.Filters;
using System.Linq.Expressions;

namespace HealthcareManagementSystem.Domain.Repository
{
    public interface IRepository<TEntity>
       where TEntity : class, new()
    {
        Task<List<TEntity>> GetItemsAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null);

        Task<List<TEntity>> GetItemsAsync<TResource>(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null);
        Task<TEntity> GetItemByIdAsync(object id);
        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        Task<int> CountAsync(
            params Expression<Func<TEntity, bool>>[] filters);

        Task<bool> ExistsAsync(
            params Expression<Func<TEntity, bool>>[] filters);

        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>>[] filters,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

        Task SaveAsync();

        void Delete(
            TEntity entity);

        void Update(
            TEntity entity
            );

        void Add(
            TEntity entity
           );

        void RemoveRange(
            Func<TEntity, bool> predicate);
    }
}
