using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.User.Interfaces;

namespace BaseApp.Data.User.Repository
{
    public class UserRepository : GenericRepository<Models.User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<bool> IsExistingUser(string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsUserNameTaken(int id, string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName && u.Id != id);
        }
    }
}
