using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.User.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        public async Task<bool> IsUserNameTaken(string id, string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName && u.Id != id);
        }

        public async Task<Models.User> GetUserAsync(string id)
        {
            return await this.GetByConditionAsync(u => u.Id == id).FirstOrDefaultAsync();
        }
    }
}
