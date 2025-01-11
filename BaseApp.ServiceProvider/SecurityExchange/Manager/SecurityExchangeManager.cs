using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.ServiceProvider.SecurityExchange.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.ServiceProvider.SecurityExchange.Manager
{
    public class SecurityExchangeManager : ISecurityExchangeManager
    {
        private IRepositoryFactory _repository;
        private readonly ILogger _logger;

        public SecurityExchangeManager(IRepositoryFactory repository, ILogger<SecurityExchangeManager> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IRepositoryFactory Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new RepositoryFactory(Repository.Context);
                }

                return _repository;
            }
        }

    }
}
