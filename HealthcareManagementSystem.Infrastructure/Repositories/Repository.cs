using HealthcareManagementSystem.Domain.Filters;
using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthcareManagementSystem.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
      where TEntity : class, new()
    {
        public Repository(
            ApplicationDbContext context
            )
        {
            Context = context;
        }



        protected virtual ApplicationDbContext Context { get; private set; }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Context.Set<TEntity>();
            //.AsNoTracking()
        }

        public virtual async Task<TEntity> GetItemByIdAsync(object id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }
        public virtual async Task<List<TEntity>> GetItemsAsync<TResource>(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .ApplySorting<TEntity, TResource>(pagingFilter)
                .ApplyPaging(pagingFilter)
                .ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetItemsAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            ListFilter pagingFilter = null,
            Type orderByType = null,
            string orderBy = null)
        {
            var query = GetQueryable();
            if (orderByType != null &&
                 !string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderByType, orderBy);
            }
            return await query
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .ApplyPaging(pagingFilter)
                .ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> LastOrDefaultAsync(
            Expression<Func<TEntity, bool>>[] filters = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            Expression<Func<TEntity, object>> orderByDescendingColumnSelector = null)
        {
            if (orderByDescendingColumnSelector == null)
            {
                orderByDescendingColumnSelector = e => e.GetPropValue("Id");
            }

            return await GetQueryable()
                .ApplyFilters(filters)
                .ApplyIncludes(includes)
                .OrderByDescending(orderByDescendingColumnSelector)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<int> CountAsync(
            params Expression<Func<TEntity, bool>>[] filters)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .CountAsync();
        }

        public virtual async Task<bool> ExistsAsync(
            params Expression<Func<TEntity, bool>>[] filters)
        {
            return await GetQueryable()
                .ApplyFilters(filters)
                .AnyAsync();
        }

      

        public virtual async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>>[] filters,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            return await GetQueryable()
                .ApplyIncludes(includes)
                .ApplyFilters(filters)
                .AnyAsync();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Update(
            TEntity entity)
        {

            Context.Entry(entity).State = EntityState.Modified;

            Context.Set<TEntity>().Update(entity);
        }

        public void Add(
            TEntity entity
            )
        {

            Context.Entry(entity).State = EntityState.Added;
            Context.Set<TEntity>().Add(entity);
        }

        public void Delete(
            TEntity entity)
        {
            
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(
            Func<TEntity, bool> predicate)
        {
            Context.Set<TEntity>()
                .RemoveRange
                (Context.Set<TEntity>()
                    .Where(predicate));
        }

       
    }
}
