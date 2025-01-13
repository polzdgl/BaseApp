﻿using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Dtos;
using System.Net.Http.Json;
using System.Threading;

namespace BaseApp.Client.ServiceClients.Company
{
    public class CompanyProvider : ICompanyProvider
    {
        private readonly HttpClient httpClient;
        public CompanyProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<FundableCompanyDto>> GetCompaniesAsync(string nameFilter = null, CancellationToken cancellationToken = default)
        {
            try
            {
                string queryString = string.IsNullOrWhiteSpace(nameFilter) ? "" : $"?startsWith={Uri.EscapeDataString(nameFilter)}";

                var response = await httpClient.GetFromJsonAsync<IEnumerable<FundableCompanyDto>>($"/api/company/companies{queryString}", cancellationToken);

                return response ?? Enumerable.Empty<FundableCompanyDto>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred trying to retrieve Company list!", ex);
            }
        }

        public async Task ImportMarketDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.PostAsync("/api/company/importMarketData", null, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to import market data. Status: {response.StatusCode}, Details: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while trying to import market data!", ex);
            }
        }

        public async Task<bool> IsMarketDataLoaded(CancellationToken cancellationToken = default)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<bool>($"/api/company/isMarketDataLoaded", cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred trying to get Market Data load status!", ex);
            }
        }
    }
}
