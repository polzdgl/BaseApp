using BaseApp.Data.Company.Dtos;
using BaseApp.Data.Company.Models;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ICompanyManager
    {
        Task<CikImportResult> ImportMarketDataAsync();
        Task<CikImportResult> ImportCompnanyDataAsync(IEnumerable<string> ciks);
        Task<PaginatedResult<FundableCompanyDto>> GetCompaniesAsync(int page, int pageSize, string? startsWith = null);
        decimal CalculateStandardFundableAmount(IEnumerable<InfoFactUsGaapIncomeLossUnitsUsd> incomeData);
        int GetYearFromFrame(string frame);
        decimal CalculateSpecialFundableAmount(decimal standardAmount, string name, decimal income2021, decimal income2022);
        Task CreateMarketDataLoadRecord();
        Task<bool> IsMarketDataLoadedAsync();
        Task ImportPublicCompanies();
    }
}
