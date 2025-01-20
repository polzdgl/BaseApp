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
        private readonly BulkConfig _defaultBulkConfig;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _defaultBulkConfig = new BulkConfig 
            { 
                UseTempDB = true,
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
            };
        }

        #region CREATE

        public virtual bool Create(T entity, bool saveChanges = true)
        {
            _dbContext.Set<T>().Add(entity);
            if (saveChanges)
            {
                return SaveChanges() > 0;
            }
            return true;
        }

        public virtual async Task<bool> CreateAsync(T entity, bool saveChanges = true)
        {
            _dbContext.Set<T>().Add(entity);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return true;
        }

        public virtual bool CreateAll(IEnumerable<T> entityList, bool saveChanges = true)
        {
            _dbContext.Set<T>().AddRange(entityList);
            if (saveChanges)
            {
                return SaveChanges() > 0;
            }
            return true;
        }

        public virtual async Task<bool> CreateAllAsync(IEnumerable<T> entityList, bool saveChanges = true)
        {
            await _dbContext.Set<T>().AddRangeAsync(entityList);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return true;
        }

        public virtual bool BulkCreateAll(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.BulkInsert(entityList, bulkConfig ?? _defaultBulkConfig);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                // throw error to surface the exception
                throw;  
            }
        }

        public virtual async Task<bool> BulkCreateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _dbContext.BulkInsertAsync(entityList, bulkConfig ?? _defaultBulkConfig);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                // throw error to surface the exception
                throw;
            }
        }

        #endregion

        #region READ

        public virtual T Single(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Single(predicate);
        }

        public virtual async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().SingleAsync(predicate);
        }

        public virtual T? FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual T First(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().First(predicate);
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstAsync(predicate);
        }

        public virtual T? SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().SingleOrDefault(predicate);
        }

        public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<T?> FindAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T?> FindAsync(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        // Removed the unused 'includeHistory' parameter
        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression);
        }

        public virtual async Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>>? includeLambdas = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(expression);

            if (includeLambdas != null)
            {
                includeLambdas.ForEach(include => query = query.Include(include));
            }

            return await query.ToListAsync();
        }

        public virtual bool Any(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Any(expression);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().AnyAsync(expression);
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
        {
            return await _dbContext.Set<T>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        #endregion

        #region FILTERING

        public virtual IQueryable<T> FilterBy(IQueryable<T> entityList, Expression<Func<T, bool>> filterExpression)
        {
            return entityList.Where(filterExpression);
        }

        public virtual async Task<IList<T>> FilterByAsync(IQueryable<T> entityList, Expression<Func<T, bool>> filterExpression)
        {
            return await entityList.Where(filterExpression).ToListAsync();
        }

        #endregion

        #region UPDATE

        public virtual bool Update(T entity, bool saveChanges = true)
        {
            _dbContext.Set<T>().Update(entity);
            if (saveChanges)
            {
                return SaveChanges() > 0;
            }
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity, bool saveChanges = true)
        {
            _dbContext.Set<T>().Update(entity);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return true;
        }

        public virtual bool UpdateAll(IEnumerable<T> entityList, bool saveChanges = true)
        {
            _dbContext.UpdateRange(entityList);
            if (saveChanges)
            {
                return SaveChanges() > 0;
            }
            return true;
        }

        public virtual async Task<bool> UpdateAllAsync(IEnumerable<T> entityList, bool saveChanges = true)
        {
            _dbContext.UpdateRange(entityList);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return true;
        }

        public virtual bool BulkUpdateAll(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.BulkUpdate(entityList, bulkConfig ?? _defaultBulkConfig);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                // throw error to surface the exception
                throw;
            }
        }

        public virtual async Task<bool> BulkUpdateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _dbContext.BulkUpdateAsync(entityList, bulkConfig ?? _defaultBulkConfig);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                // throw error to surface the exception
                throw;
            }
        }

        #endregion

        #region DELETE

        public virtual bool Delete(T entity, bool saveChanges = true)
        {
            _dbContext.Set<T>().Remove(entity);
            if (saveChanges)
            {
                return SaveChanges() > 0;
            }
            return true;
        }

        public virtual async Task<bool> DeleteAsync(T entity, bool saveChanges = true)
        {
            _dbContext.Set<T>().Remove(entity);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return true;
        }

        public virtual bool DeleteAll(IEnumerable<T> entityList, bool saveChanges = true)
        {
            _dbContext.Set<T>().RemoveRange(entityList);
            if (saveChanges)
            {
                return SaveChanges() > 0;
            }
            return true;
        }

        public virtual async Task<bool> DeleteAllAsync(IEnumerable<T> entityList, bool saveChanges = true)
        {
            _dbContext.Set<T>().RemoveRange(entityList);
            if (saveChanges)
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return true;
        }

        public virtual bool BulkDeleteAll(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.BulkDelete(entityList, bulkConfig ?? _defaultBulkConfig);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                // throw error to surface the exception
                throw;
            }
        }

        public virtual async Task<bool> BulkDeleteAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _dbContext.BulkDeleteAsync(entityList, bulkConfig ?? _defaultBulkConfig);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                // throw error to surface the exception
                throw;
            }
        }

        #endregion

        #region SAVE / ROLLBACK

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public virtual void RollBack()
        {
            IEnumerable<EntityEntry> changedEntries = _dbContext.ChangeTracker
                .Entries()
                .Where(x => x.State != EntityState.Unchanged)
                .ToList();

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

        #endregion
    }
}
