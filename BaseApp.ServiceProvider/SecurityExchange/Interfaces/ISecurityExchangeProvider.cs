using BaseApp.Data.SecurityExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.ServiceProvider.SecurityExchange.Interfaces
{
    public interface ISecurityExchangeProvider
    {
        Task<EdgarCompanyInfo> FetchEdgarCompanyInfoAsync(string cik);
    }
}
