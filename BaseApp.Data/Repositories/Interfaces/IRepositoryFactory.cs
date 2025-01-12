using BaseApp.Data.Context;
using BaseApp.Data.SecurityExchange.Interfaces;
using BaseApp.Data.User.Interfaces;

namespace BaseApp.Data.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        ApplicationDbContext Context { get; set; }

        IUserRepository UserRepository { get; }

        ISecurityExchangeRepository SecurityExchangeRepository { get; }
    }
}
