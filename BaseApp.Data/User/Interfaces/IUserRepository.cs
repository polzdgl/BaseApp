using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.User.Models;

namespace BaseApp.Data.User.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<bool> IsExistingUserAsync(string? userName);
        Task<bool> IsUserNameTakenAsync(string id, string? userName);
        Task<ApplicationUser?> GetUserAsync(string id);
    }
}
