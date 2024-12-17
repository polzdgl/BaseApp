using BaseApp.Data.Repositories.Interfaces;

namespace BaseApp.Data.User.Interfaces
{
    public interface IUserRepository : IGenericRepository<Models.User>
    {
        //Task<IEnumerable<Models.User>> GetAllAsync();
        //Task<Models.User> GetByIdAsync(int id);
        //Task AddAsync(Models.User entity);
        //Task UpdateAsync(Models.User entity);
        //Task DeleteAsync(Models.User entity);
        //Task<Models.User> GetByUserId(int id);
    }
}
