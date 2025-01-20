using BaseApp.Data.Company.Dtos;
using BaseApp.Data.Company.Models;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ISecurityExchangeProvider
    {
        Task<CompanyInfo> FetchEdgarCompanyInfoAsync(string cik);
        Task<List<PublicCompanyDto>> FetchAllPublicCompanies();
        Task<List<PublicCompanyDto>> FetchPublicCompaniesByCik(IEnumerable<int> ciks);
    }
}
