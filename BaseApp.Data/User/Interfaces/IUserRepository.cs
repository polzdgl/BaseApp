using BaseApp.Data.Repositories.Interfaces;

namespace BaseApp.Data.User.Interfaces
{
    public interface IUserRepository : IGenericRepository<Models.User>
    {
        Task<bool> IsExistingUser(string? userName);
    }
}
