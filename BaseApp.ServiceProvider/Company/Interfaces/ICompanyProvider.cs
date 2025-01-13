using BaseApp.Data.SecurityExchange.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ICompanyProvider
    {
        Task<IEnumerable<FundableCompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken = default);
    }
}
