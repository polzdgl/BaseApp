using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.SecurityExchange.Models;
using BaseApp.Data.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.SecurityExchange.Interfaces
{
    public interface ISecurityExchangeRepository : IGenericRepository<EdgarCompanyInfo>
    {
        Task<IEnumerable<EdgarCompanyInfo>> GetCompaniesWithDetails(string? startsWith = null);
    }
}
