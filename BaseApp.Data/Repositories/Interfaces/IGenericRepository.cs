using EFCore.BulkExtensions;
using System.Linq.Expressions;

namespace BaseApp.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool includeHistory = false);
        bool Any(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> FilterBy(IQueryable<T> entityList, Expression<Func<T, bool>> filterExpression);
        bool Create(T entity, bool saveChanges = true);
        Task<bool> CreateAsync(T entity, bool saveChanges = true);
        bool CreateAll(IEnumerable<T> entityList, bool saveChanges = true);
        Task<bool> CreateAllAsync(IEnumerable<T> entityList, bool saveChanges = true);
        bool BulkCreateAll(IList<T> entityList, BulkConfig? bulkConfig = null);
        Task<bool> BulkCreateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null);
        bool BulkUpdateAll(IList<T> entityList, BulkConfig? bulkConfig = null);
        Task<bool> BulkUpdateAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null);
        bool BulkDeleteAll(IList<T> entityList, BulkConfig? bulkConfig = null);
        Task<bool> BulkDeleteAllAsync(IList<T> entityList, BulkConfig? bulkConfig = null);
        bool Update(T entity, bool saveChanges = true);
        Task<bool> UpdateAsync(T entity, bool saveChanges = true);
        bool UpdateAll(IEnumerable<T> entityList, bool saveChanges = true);
        Task<bool> UpdateAllAsync(IEnumerable<T> entityList, bool saveChanges = true);
        bool Delete(T entity, bool saveChanges = true);
        Task<bool> DeleteAsync(T entity, bool saveChanges = true);
        bool DeleteAll(IEnumerable<T> entityList, bool saveChanges = true);
        Task<bool> DeleteAllAsync(IEnumerable<T> entityList, bool saveChanges = true);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        T Single(Expression<Func<T, bool>> predicate);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(int id);
        void RollBack();
    }
}
