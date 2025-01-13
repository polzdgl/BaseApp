using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.User.Interfaces;
using BaseApp.Data.User.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.User.Repository
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        // Checks if username already exist in the database
        public async Task<bool> IsExistingUserAsync(string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName);
        }

        // Checks if username is available, when updating user profile
        public async Task<bool> IsUserNameTakenAsync(string id, string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName && u.Id != id);
        }

        // Get user by id
        public async Task<ApplicationUser?> GetUserAsync(string id)
        {
            return await this.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
