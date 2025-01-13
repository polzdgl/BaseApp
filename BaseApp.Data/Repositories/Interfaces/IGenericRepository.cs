using EFCore.BulkExtensions;
using System.Linq.Expressions;

namespace BaseApp.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        #region CREATE

        bool Create(T entity, bool saveChanges = true);
        Task<bool> CreateAsync(T entity, bool saveChanges = true);

        bool CreateAll(IEnumerable<T> entityList, bool saveChanges = true);
        Task<bool> CreateAllAsync(IEnumerable<T> entityList, bool saveChanges = true);

        bool BulkCreateAll(IList<T> entityList, BulkConfig? bulkConfig = null);
        Task<bool> BulkCreateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null);

        #endregion

        #region READ

        T Single(Expression<Func<T, bool>> predicate);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);

        T? SingleOrDefault(Expression<Func<T, bool>> predicate);
        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        T First(Expression<Func<T, bool>> predicate);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);

        T? FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<T?> FindAsync(int id);
        Task<T?> FindAsync(string id);

        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> expression,
                                          List<Expression<Func<T, object>>>? includeLambdas = null);

        bool Any(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<int> CountAsync();

        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);

        #endregion

        #region FILTERING

        IQueryable<T> FilterBy(IQueryable<T> entityList, Expression<Func<T, bool>> filterExpression);
        Task<IList<T>> FilterByAsync(IQueryable<T> entityList, Expression<Func<T, bool>> filterExpression);

        #endregion

        #region UPDATE

        bool Update(T entity, bool saveChanges = true);
        Task<bool> UpdateAsync(T entity, bool saveChanges = true);

        bool UpdateAll(IEnumerable<T> entityList, bool saveChanges = true);
        Task<bool> UpdateAllAsync(IEnumerable<T> entityList, bool saveChanges = true);

        bool BulkUpdateAll(IList<T> entityList, BulkConfig? bulkConfig = null);
        Task<bool> BulkUpdateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null);

        #endregion

        #region DELETE

        bool Delete(T entity, bool saveChanges = true);
        Task<bool> DeleteAsync(T entity, bool saveChanges = true);

        bool DeleteAll(IEnumerable<T> entityList, bool saveChanges = true);
        Task<bool> DeleteAllAsync(IEnumerable<T> entityList, bool saveChanges = true);

        bool BulkDeleteAll(IList<T> entityList, BulkConfig? bulkConfig = null);
        Task<bool> BulkDeleteAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null);

        #endregion

        #region SAVE / ROLLBACK

        int SaveChanges();
        Task<int> SaveChangesAsync();

        void RollBack();

        #endregion
    }
}
