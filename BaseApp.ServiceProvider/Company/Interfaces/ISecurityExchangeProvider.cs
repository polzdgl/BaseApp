using BaseApp.Data.SecurityExchange.Models;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ISecurityExchangeProvider
    {
        Task<EdgarCompanyInfo> FetchEdgarCompanyInfoAsync(string cik);
    }
}
