using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.Data.SecurityExchange.Models;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ICompanyManager
    {
        Task ImportMarketDataAsync();
        Task ImportCompnanyDataAsync(IEnumerable<string> ciks);
        Task<List<FundableCompanyDto>> GetCompanies(string? startsWith = null);
        decimal CalculateStandardFundableAmount(IEnumerable<InfoFactUsGaapIncomeLossUnitsUsd> incomeData);
        int GetYearFromFrame(string frame);
        decimal CalculateSpecialFundableAmount(decimal standardAmount, string name, decimal income2021, decimal income2022);
        Task CreateMarketDataLoadRecord();
        Task<bool> IsMarketDataLoadedAsync();
    }
}
