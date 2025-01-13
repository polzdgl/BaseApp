using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Dtos;
using System.Net.Http.Json;

namespace BaseApp.Client.ServiceClients.Company
{
    public class CompanyProvider : ICompanyProvider
    {
        private readonly HttpClient httpClient;
        public CompanyProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<FundableCompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<IEnumerable<FundableCompanyDto>>($"/api/Compnany/companies", cancellationToken);

                return response ?? Enumerable.Empty<FundableCompanyDto>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred trying to retrieve Company list!", ex);
            }
        }
    }
}
