using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.ServiceProvider.SecurityExchange.Interfaces
{
    public interface ISecurityExchangeManager
    {
        Task ImportCompnanyDataAsync(IEnumerable<string> ciks);
        Task<List<FundableCompanyDto>> GetFunableCompanies(string? startsWith = null);
    }
}
