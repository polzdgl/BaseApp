using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.Data.SecurityExchange.Dtos;
using BaseApp.Data.SecurityExchange.Models;
using BaseApp.ServiceProvider.SecurityExchange.Interfaces;
using Microsoft.Extensions.Logging;

namespace BaseApp.ServiceProvider.SecurityExchange.Manager
{
    public class SecurityExchangeManager : ISecurityExchangeManager
    {
        private IRepositoryFactory _repository;
        private ISecurityExchangeProvider _securityExchangeProvider;
        private readonly ILogger _logger;

        public SecurityExchangeManager(IRepositoryFactory repository, ISecurityExchangeProvider securityExchangeProvider,
            ILogger<SecurityExchangeManager> logger)
        {
            _repository = repository;
            _securityExchangeProvider = securityExchangeProvider;
            _logger = logger;
        }

        public IRepositoryFactory Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new RepositoryFactory(Repository.Context);
                }

                return _repository;
            }
        }

        public async Task ImportCompnanyDataAsync(IEnumerable<string> ciks)
        {
            List<EdgarCompanyInfo> companies = new List<EdgarCompanyInfo>();

            foreach (var cik in ciks)
            {
                var data = await _securityExchangeProvider.FetchEdgarCompanyInfoAsync(cik);

                // Filter relevant data
                var company = new EdgarCompanyInfo
                {
                    Cik = data.Cik,
                    EntityName = data.EntityName,
                    InfoFact = new InfoFact
                    {
                        InfoFactUsGaap = new InfoFactUsGaap
                        {
                            InfoFactUsGaapNetIncomeLoss = new InfoFactUsGaapNetIncomeLoss
                            {
                                InfoFactUsGaapIncomeLossUnits = new InfoFactUsGaapIncomeLossUnits
                                {
                                    InfoFactUsGaapIncomeLossUnitsUsd = data.InfoFact?.InfoFactUsGaap?.InfoFactUsGaapNetIncomeLoss?
                                    .InfoFactUsGaapIncomeLossUnits?.InfoFactUsGaapIncomeLossUnitsUsd?
                                    .Where(usd => usd.Form == "10-K" && (usd.Frame?.StartsWith("CY") ?? false))
                                    .ToArray() ?? Array.Empty<InfoFactUsGaapIncomeLossUnitsUsd>()
                                }
                            }
                        }
                    }
                };
                companies.Add(company);
            }

            // Save to database
            await Repository.SecurityExchangeRepository.CreateAllAsync(companies);
        }

        public async Task<List<FundableCompanyDto>> GetFunableCompanies(string? startsWith = null)
        {
            var fundableCompanies = new List<FundableCompanyDto>();

            var companies = await Repository.SecurityExchangeRepository.GetCompaniesWithDetails(startsWith);

            if (companies.Any())
            {
                foreach (var company in companies)
                {
                    var incomeData = company?.InfoFact?.InfoFactUsGaap?.InfoFactUsGaapNetIncomeLoss?.InfoFactUsGaapIncomeLossUnits?.InfoFactUsGaapIncomeLossUnitsUsd;
                    var incomeDataList = incomeData ?? new List<InfoFactUsGaapIncomeLossUnitsUsd>();

                    var standardAmount = this.CalculateStandardFundableAmount(incomeDataList);

                    var income2021 = incomeDataList.FirstOrDefault(d => d.Frame == "CY2021")?.Val ?? 0;
                    var income2022 = incomeDataList.FirstOrDefault(d => d.Frame == "CY2022")?.Val ?? 0;

                    var specialAmount = this.CalculateSpecialFundableAmount(standardAmount, company?.EntityName ?? "Unknown", income2021, income2022);

                    fundableCompanies.Add(new FundableCompanyDto
                    {
                        Id = company.Cik,
                        Name = company.EntityName,
                        StandardFundableAmount = standardAmount,
                        SpecialFundableAmount = specialAmount
                    });
                }
            }

            return fundableCompanies;
        }

        private decimal CalculateStandardFundableAmount(IEnumerable<InfoFactUsGaapIncomeLossUnitsUsd> incomeData)
        {
            var targetYears = new[] { 2018, 2019, 2020, 2021, 2022 };

            // Filter for yearly data only (e.g., "CY2018", not "CY2018Q1")
            var yearlyData = incomeData
                .Where(usd => targetYears.Contains(GetYearFromFrame(usd.Frame)))
                .GroupBy(usd => GetYearFromFrame(usd.Frame)) // Ensure only one entry per year
                .Select(g => g.FirstOrDefault()) // Pick the first entry for the year
                .ToList();

            // Check if we have data for all required years
            if (yearlyData.Count != targetYears.Length) return 0;

            // Ensure positive income for specific years
            if (!yearlyData.Any(d => d.Frame.StartsWith("CY2021") && d.Val > 0) ||
                !yearlyData.Any(d => d.Frame.StartsWith("CY2022") && d.Val > 0)) return 0;

            // Calculate highest income
            var highestIncome = yearlyData.Max(d => d.Val);

            // Apply different rates based on income threshold
            return highestIncome >= 10_000_000_000
                ? highestIncome * 0.1233m
                : highestIncome * 0.2151m;
        }

        // Helper method to extract the year from the frame
        private int GetYearFromFrame(string frame)
        {
            // Extract numeric year from frame (e.g., "CY2018" or "CY2018Q1")
            var match = System.Text.RegularExpressions.Regex.Match(frame, @"CY(\d{4})");
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }

        private decimal CalculateSpecialFundableAmount(decimal standardAmount, string name, decimal income2021, decimal income2022)
        {
            var specialAmount = standardAmount;

            if ("AEIOU".Contains(name[0])) specialAmount += standardAmount * 0.15m;
            if (income2022 < income2021) specialAmount -= standardAmount * 0.25m;

            return specialAmount;
        }
    }
}
