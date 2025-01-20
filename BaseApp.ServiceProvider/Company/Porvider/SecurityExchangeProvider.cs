using BaseApp.Data.Company.Models;
using BaseApp.Data.Config.SecurityExchnage;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Enums.Compnay;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaseApp.ServiceProvider.Company.Porvider
{
    public class SecurityExchangeProvider : ISecurityExchangeProvider
    {
        // Todo: Inject HttpClient with StandardResilency in Program.cs
        private readonly HttpClient _httpClient;
        private readonly SecApiSettings _secApiSettings;
        private readonly ILogger<SecurityExchangeProvider> _logger;

        public SecurityExchangeProvider(HttpClient httpClient, IOptions<SecApiSettings> secApiSettings, ILogger<SecurityExchangeProvider> logger)
        {
            _secApiSettings = secApiSettings.Value;
            _logger = logger;

            _httpClient = httpClient;

            // Add required headers to HttpClient
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.34.0");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
        }

        // Get Company Info from SEC API
        public async Task<CompanyInfo> FetchEdgarCompanyInfoAsync(string cik)
        {
            // Build request url and make sure cik is 10 characters long
            var url = $"{_secApiSettings.BaseUrl}{_secApiSettings.EdgarCompanyInfoUrl}{cik.PadLeft((int)CikPaddingEnum.PaddingNumber, (char)CikPaddingEnum.PaddingValue)}.json";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            return JsonSerializer.Deserialize<CompanyInfo>(content, options);
        }

        // Get all public companies from SEC API / Json file
        public async Task<List<PublicCompany>> FetchAllPublicCompanies()
        {
            string content;

            // Build the URL using config settings.
            var url = $"{_secApiSettings.CompanyTickers}.json";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(response.Content.ToString(), "API call to fetch public companies failed. Loading local Tickers.json instead.");

                var filePath = Path.Combine(AppContext.BaseDirectory, "StaticData", "Tickers.json");
                content = await File.ReadAllTextAsync(filePath);
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true
            };

            // Deserialize the JSON into a dictionary.
            var companiesDict = JsonSerializer.Deserialize<Dictionary<string, PublicCompany>>(content, options);

            var companies = companiesDict?.Values.ToList() ?? new List<PublicCompany>();

            return companies;
        }

        // Get public companies by CIKs from SEC API / Json file
        public async Task<List<PublicCompany>> FetchPublicCompaniesByCik(IEnumerable<int> ciks)
        {
            string content;

            // Build the URL using config settings.
            var url = $"{_secApiSettings.CompanyTickers}.json";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(response.Content.ToString(), "API call to fetch public companies failed. Loading local Tickers.json instead.");

                var filePath = Path.Combine(AppContext.BaseDirectory, "StaticData", "Tickers.json");
                content = await File.ReadAllTextAsync(filePath);
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true
            };

            // Deserialize the JSON into a dictionary.
            var companies = JsonSerializer.Deserialize<List<PublicCompany>>(content, options)
                .Where(c => ciks.Contains(c.Cik)) ?? new List<PublicCompany>();

            var filteredCompanies = companies.Where(c => ciks.Contains(c.Cik));

            return companies.ToList();
        }
    }
}
