using BaseApp.Data.Company.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.Company.Interfaces
{
    public interface ICompanyClient
    {
        Task<PaginatedResult<FundableCompanyDto>> GetCompaniesAsync(int page, int pageSize, string? nameFilter, CancellationToken cancellationToken = default);
        Task ImportMarketDataAsync(CancellationToken cancellationToken = default);
        Task<bool> IsMarketDataLoaded(CancellationToken cancellationToken = default);
    }
}
