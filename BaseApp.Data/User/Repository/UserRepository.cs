using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.User.Interfaces;

namespace BaseApp.Data.User.Repository
{
    public class UserRepository : GenericRepository<Models.User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        //public IRepositoryFactory Repository
        //{
        //    get
        //    {
        //        if (this.repository == null)
        //        {
        //            this.repository = new RepositoryFactory(_context);
        //        }

        //        return this.repository;
        //    }
        //}

        //public async Task<Models.User> GetByUserId(int id)
        //{
        //    return await this.FindAsync(id);
        //}

        //// Get all entities
        //public async Task<IEnumerable<T>> GetAllAsync()
        //{
        //    return await _context.get();
        //}

        //// Get an entity by its ID
        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}

        //// Add a new entity
        //public async Task AddAsync(T entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    await _context.SaveChangesAsync();
        //}

        //// Update an existing entity
        //public async Task UpdateAsync(T entity)
        //{
        //    _dbSet.Attach(entity);
        //    _context.Entry(entity).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //// Delete an entity
        //public async Task DeleteAsync(T entity)
        //{
        //    _dbSet.Remove(entity);
        //    await _context.SaveChangesAsync();
        //}

        //Task<Models.User> IUserRepository.GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task AddAsync(Models.User entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateAsync(Models.User entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(Models.User entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
