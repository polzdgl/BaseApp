﻿using BaseApp.Data.SecurityExchange.Models;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Enums.Compnay;
using System.Text.Json;

namespace BaseApp.ServiceProvider.Company.Porvider
{
    public class SecurityExchangeProvider : ISecurityExchangeProvider
    {
        // Todo: Inject HttpClient with StandardResilency in Program.cs
        private readonly HttpClient _httpClient;

        // Todo: Retrive this from AppSettings 
        private const string BaseUrl = "https://data.sec.gov/api/xbrl/companyfacts/CIK";

        public SecurityExchangeProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Add required headers to HttpClient
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.34.0");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
        }

        // Get Company Info from SEC API
        public async Task<EdgarCompanyInfo> FetchEdgarCompanyInfoAsync(string cik)
        {
            // Build request url and make sure cik is 10 characters long
            var url = $"{BaseUrl}{cik.PadLeft((int)CikPaddingEnum.PaddingNumber, (char)CikPaddingEnum.PaddingValue)}.json";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            return JsonSerializer.Deserialize<EdgarCompanyInfo>(content, options);
        }
    }
}
