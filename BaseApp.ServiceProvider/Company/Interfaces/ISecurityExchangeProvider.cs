using BaseApp.Data.Company.Models;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ISecurityExchangeProvider
    {
        Task<CompanyInfo> FetchEdgarCompanyInfoAsync(string cik);
        Task<List<PublicCompany>> FetchAllPublicCompanies();
        Task<List<PublicCompany>> FetchPublicCompaniesByCik(IEnumerable<int> ciks);
    }
}
