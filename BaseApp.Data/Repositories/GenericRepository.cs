using BaseApp.Data.Context;
using BaseApp.Data.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace BaseApp.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public virtual bool Create(T entity, bool saveChanges = true)
        {
            this._dbContext.Set<T>().Add(entity);
            if (saveChanges)
            {
                return this.SaveChanges() > 0;
            }
            else
            {
                return true;
            }
        }

        public async virtual Task<bool> CreateAsync(T entity, bool saveChanges = true)
        {
            this._dbContext.Set<T>().Add(entity);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }

        public virtual bool CreateAll(IEnumerable<T> entityList, bool saveChanges = true)
        {
            this._dbContext.Set<T>().AddRange(entityList);
            if (saveChanges)
            {
                return this.SaveChanges() > 0;
            }
            else
            {
                return true;
            }
        }

        public async virtual Task<bool> CreateAllAsync(IEnumerable<T> entityList, bool saveChanges = true)
        {
            await this._dbContext.Set<T>().AddRangeAsync(entityList);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }

        public virtual bool BulkCreateAll(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            var type = typeof(T);

            this._dbContext.BulkInsert(entityList, bulkConfig);

            return true;
        }

        public async virtual Task<bool> BulkCreateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            var type = typeof(T);

            await this._dbContext.BulkInsertAsync(entityList, bulkConfig);

            return true;
        }

        public virtual bool BulkUpdateAll(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            this._dbContext.BulkUpdate(entityList, bulkConfig);

            return true;
        }

        public async virtual Task<bool> BulkUpdateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            await _dbContext.BulkUpdateAsync(entityList, bulkConfig);

            return true;
        }

        public virtual bool BulkDeleteAll(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            return this.BulkUpdateAll(entityList, bulkConfig);
        }

        public async virtual Task<bool> BulkDeleteAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            return await BulkUpdateAllAsync(entityList, bulkConfig);
        }
        public virtual bool Delete(T entity, bool saveChanges = true)
        {
            this._dbContext.Set<T>().Remove(entity);
            if (saveChanges)
            {
                return this.SaveChanges() > 0;
            }
            else
            {
                return true;
            }
        }

        public async virtual Task<bool> DeleteAsync(T entity, bool saveChanges = true)
        {
            this._dbContext.Set<T>().Remove(entity);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }

        public virtual bool DeleteAll(IEnumerable<T> entityList, bool saveChanges = true)
        {
            this._dbContext.Set<T>().RemoveRange(entityList);
            if (saveChanges)
            {
                return this.SaveChanges() > 0;
            }
            else
            {
                return true;
            }
        }

        public async virtual Task<bool> DeleteAllAsync(IEnumerable<T> entityList, bool saveChanges = true)
        {
            this._dbContext.Set<T>().RemoveRange(entityList);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }

        public virtual T Single(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Single(predicate);
        }

        public async virtual Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().SingleAsync(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public async virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public virtual T First(Expression<Func<T, bool>> predicate)
        {
            return this._dbContext.Set<T>().First(predicate);
        }

        public async virtual Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await this._dbContext.Set<T>().FirstAsync(predicate);
        }

        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this._dbContext.Set<T>().SingleOrDefault(predicate);
        }

        public async virtual Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await this._dbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public async virtual Task<T> FindAsync(int id)
        {
            return await this._dbContext.Set<T>().FindAsync(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return this._dbContext.Set<T>();
        }
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._dbContext.Set<T>().ToListAsync();
        }

        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool includeHistory = false)
        {
            var query = this._dbContext.Set<T>()
                        .Where(expression);

            return query;
        }

        public virtual IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includeLambdas = null)
        {
            var query = this._dbContext.Set<T>()
                        .Where(expression);

            if (includeLambdas != null)
            {
                includeLambdas.ForEach((Expression<Func<T, object>> includeExpression) => query = query.Include(includeExpression));
            }

            return query;
        }

        public virtual bool Any(Expression<Func<T, bool>> expression)
        {
            return this._dbContext.Set<T>().Any(expression);
        }

        public async virtual Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await this._dbContext.Set<T>().AnyAsync(expression);
        }

        public virtual IQueryable<T> FilterBy(IQueryable<T> entityList, Expression<Func<T, bool>> filterExpression)
        {
            var query = entityList.Where(filterExpression);
            return query;
        }

        public virtual bool Update(T entity, bool saveChanges = true)
        {
            this._dbContext.Set<T>().Update(entity);
            if (saveChanges)
            {
                return this.SaveChanges() > 0;
            }
            else
            {
                return true;
            }
        }

        public async virtual Task<bool> UpdateAsync(T entity, bool saveChanges = true)
        {
            this._dbContext.Set<T>().Update(entity);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }

        public virtual bool UpdateAll(IEnumerable<T> entityList, bool saveChanges = true)
        {
            this._dbContext.UpdateRange(entityList);

            if (saveChanges)
            {
                return this.SaveChanges() > 0;
            }
            else
            {
                return true;
            }
        }

        public async virtual Task<bool> UpdateAllAsync(IEnumerable<T> entityList, bool saveChanges = true)
        {
            this._dbContext.UpdateRange(entityList);

            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }
        public virtual int SaveChanges()
        {
            return this._dbContext.SaveChanges();
        }

        public async virtual Task<int> SaveChangesAsync()
        {
            return await this._dbContext.SaveChangesAsync();
        }

        public virtual void RollBack()
        {
            IEnumerable<EntityEntry> changedEntries = this._dbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (EntityEntry entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
