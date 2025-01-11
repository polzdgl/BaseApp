using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.SecurityExchange.Interfaces;
using BaseApp.Data.SecurityExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.SecurityExchange.Repository
{
    public class SecurityExchangeRepository : GenericRepository<EdgarCompanyInfo>, ISecurityExchangeRepository
    {
        public SecurityExchangeRepository(ApplicationDbContext context)
            : base(context) 
        {
            
        }


    }
}
