using BaseApp.Data.Company.Models;
using BaseApp.Data.Repositories.Interfaces;

namespace BaseApp.Data.Company.Interfaces
{
    public interface IEdgarCompanyInfoRepository : IGenericRepository<EdgarCompanyInfo>
    {
        IEnumerable<string> GetCiksToImport();
        Task<IEnumerable<string>> GetAllCikIds();
        Task<IEnumerable<EdgarCompanyInfo>> GetCompaniesWithDetails(string? startsWith = null);
    }
}
