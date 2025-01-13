using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.SecurityExchange.Models;

namespace BaseApp.Data.SecurityExchange.Interfaces
{
    public interface IEdgarCompanyInfoRepository : IGenericRepository<EdgarCompanyInfo>
    {
        IEnumerable<string> GetCiksToImport();
        Task<IEnumerable<string>> GetAllCikIds();
        Task<IEnumerable<EdgarCompanyInfo>> GetCompaniesWithDetails(string? startsWith = null);
    }
}
