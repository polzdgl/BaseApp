using BaseApp.Data.Company.Dtos;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ICompanyClient
    {
        Task<IEnumerable<FundableCompanyDto>> GetCompaniesAsync(string? nameFilter, CancellationToken cancellationToken = default);
        Task<CompanyDetailsDto> GetCompanyDetailsAsync(string id, CancellationToken cancellationToken = default);
        Task ImportMarketDataAsync(CancellationToken cancellationToken = default);
        Task<bool> IsMarketDataLoaded(CancellationToken cancellationToken = default);
    }
}
