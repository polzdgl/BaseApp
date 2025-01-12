using BaseApp.Data.Context;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.SecurityExchange.Interfaces;
using BaseApp.Data.SecurityExchange.Repository;
using BaseApp.Data.User.Interfaces;
using BaseApp.Data.User.Repository;

namespace BaseApp.Data.Repositories
{
    public class RepositoryFactory(ApplicationDbContext context) : IRepositoryFactory
    {
        private IUserRepository _userRepository;
        private ISecurityExchangeRepository _securityExchangeRepository;

        public ApplicationDbContext Context { get; set; } = context;

        public IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(this.Context);
                }

                return this._userRepository;
            }
        }

        public ISecurityExchangeRepository SecurityExchangeRepository
        {
            get
            {
                if (this._securityExchangeRepository == null)
                {
                    this._securityExchangeRepository = new SecurityExchangeRepository(this.Context);
                }

                return this._securityExchangeRepository;
            }
        }
    }
}
