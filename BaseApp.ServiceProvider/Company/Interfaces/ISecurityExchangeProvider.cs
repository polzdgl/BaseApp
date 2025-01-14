using BaseApp.Data.Company.Models;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ISecurityExchangeProvider
    {
        Task<EdgarCompanyInfo> FetchEdgarCompanyInfoAsync(string cik);
    }
}
