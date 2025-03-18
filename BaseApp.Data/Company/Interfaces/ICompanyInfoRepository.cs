using BaseApp.Data.Company.Models;
using BaseApp.Data.Repositories.Interfaces;

namespace BaseApp.Data.Company.Interfaces
{
    public interface ICompanyInfoRepository : IGenericRepository<CompanyInfo>
    {
        Task<IEnumerable<string>> GetCiksToImportAsync();
        Task<IEnumerable<string>> GetAllCikIds();
        Task<IEnumerable<CompanyInfo>> GetCompaniesWithDetails(string? startsWith = null);
        Task<CompanyInfo?> GetCompanyWithDetailsByCik(int cik);
    }
}
