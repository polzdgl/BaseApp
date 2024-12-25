using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.User.Interfaces;
using BaseApp.Data.User.Models;
using BaseApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BaseApp.Data.User.Repository
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<bool> IsExistingUserAsync(string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsUserNameTakenAsync(string id, string? userName)
        {
            return await this.AnyAsync(u => u.UserName == userName && u.Id != id);
        }

        public async Task<ApplicationUser?> GetUserAsync(string id)
        {
            return await this.GetByConditionAsync(u => u.Id == id).FirstOrDefaultAsync();
        }
    }
}
