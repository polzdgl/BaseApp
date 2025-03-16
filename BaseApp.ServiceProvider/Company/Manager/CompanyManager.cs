using BaseApp.Data.Company.Dtos;
using BaseApp.Data.Company.Models;
using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.Shared.Const.Company;
using BaseApp.Shared.Enums.Compnay;
using BaseApp.Shared.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BaseApp.ServiceProvider.Company.Manager
{
    public class CompanyManager : ICompanyManager
    {
        private IRepositoryFactory _repository;
        private ISecurityExchangeProvider _securityExchangeProvider;
        private readonly ILogger _logger;

        public CompanyManager(IRepositoryFactory repository, ISecurityExchangeProvider securityExchangeProvider,
            ILogger<CompanyManager> logger)
        {
            _repository = repository;
            _securityExchangeProvider = securityExchangeProvider;
            _logger = logger;
        }

        // Target years for calculating the standard fundable amount
        private readonly HashSet<int> TargetYears = new HashSet<int>
        {
            (int)RequiredYear.Y2018,
            (int)RequiredYear.Y2019,
            (int)RequiredYear.Y2020,
            (int)RequiredYear.Y2021,
            (int)RequiredYear.Y2022
        };

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

        // Import Initial Market Data from SEC API with given CIKs
        public async Task<CikImportResult> ImportMarketDataAsync()
        {
            // Get all the public companies from SEC API / Json Data
            await this.ImportPublicCompanies();

            // Get all CIKs to import data for
            IEnumerable<string> ciksToImport = await this.Repository.CompanyInfoRepository.GetCiksToImportAsync();

            // Get all CIKs from the SEC API and save to database
            CikImportResult cikImportResult = await this.ImportCompnanyDataAsync(ciksToImport);

            // If any CIKs were successfully imported, create Market Data Load Status
            if (cikImportResult.SucceededCiks.Any())
            {
                // Create Market Data Load Status
                await this.CreateMarketDataLoadRecord();
            }

            return cikImportResult;
        }

        // Import all the company data from SEC API / Json file
        public async Task ImportPublicCompanies()
        {
            // Get all the public companies from SEC API / Json Data
            List<PublicCompany> publicCompanies = await this._securityExchangeProvider.FetchAllPublicCompanies();

            // Get existing Companies
            List<int> existingCompaniesCik = await Repository.PublicCompanyRepository.GetAll().Select(c => c.Cik).ToListAsync();

            // Filter out companies that already exist in the database
            IEnumerable<PublicCompany> newCompanies = publicCompanies.Where(c => !existingCompaniesCik.Contains(c.Cik));

            // Save new companies to the database
            if (newCompanies.Any())
            {
                // Using bulk insert to save all companies at once, and no save changes required
                await Repository.PublicCompanyRepository.BulkCreateAllAsync(newCompanies.ToList());
            }
        }

        // Import Company Data from SEC API with given CIKs
        public async Task<CikImportResult> ImportCompnanyDataAsync(IEnumerable<string> ciks)
        {
            CikImportResult cikImportResult = new CikImportResult();

            // Format to make all ciks 10 digits
            var paddedCiks = ciks.Select(cik => cik.ToPaddedCik()).Distinct();

            // Get CIKs that dont exist in the database
            var existingCiks = await Repository.CompanyInfoRepository.GetAllCikIds();

            var newCiks = paddedCiks.Except(existingCiks);

            int currentIndex = 1;
            int totalCount = newCiks.Count();
            const int batchSize = 1000;

            for (int i = 0; i < totalCount; i += batchSize)
            {
                List<CompanyInfo> companies = new List<CompanyInfo>();
                var batch = newCiks.Skip(i).Take(batchSize);

                foreach (var cik in batch)
                {
                    try
                    {
                        _logger.LogInformation("Iteration: {currentIndex}/{totalCount}. Loading Company data for CIK: {cik}", currentIndex, totalCount, cik);

                        var data = await _securityExchangeProvider.FetchEdgarCompanyInfoAsync(cik);

                        // Filter relevant data
                        var company = new CompanyInfo
                        {
                            Cik = data.Cik == 0
                            ? throw new InvalidDataException($"CIK is invalid for EntityName:{data.EntityName}, CIK: {cik}")
                            : data.Cik,

                            EntityName = string.IsNullOrEmpty(data.EntityName)
                            ? throw new InvalidDataException($"Entity Name is empty for CIK:{cik}")
                            : data.EntityName,

                            InfoFact = new InfoFact
                            {
                                InfoFactUsGaap = new InfoFactUsGaap
                                {
                                    InfoFactUsGaapNetIncomeLoss = new InfoFactUsGaapNetIncomeLoss
                                    {
                                        InfoFactUsGaapIncomeLossUnits = new InfoFactUsGaapIncomeLossUnits
                                        {
                                            //InfoFactUsGaapIncomeLossUnitsUsd = data.InfoFact?.InfoFactUsGaap?.InfoFactUsGaapNetIncomeLoss?
                                            //.InfoFactUsGaapIncomeLossUnits?.InfoFactUsGaapIncomeLossUnitsUsd?
                                            //.Where(usd => (usd.Form == FormEnum.Form10K.GetDescription() || usd.Form == FormEnum.Form10Q.GetDescription())
                                            //&& (usd.Frame?.StartsWith(FrameEnum.YearlyInfo.GetDescription()) ?? false))
                                            //.ToArray() ?? Array.Empty<InfoFactUsGaapIncomeLossUnitsUsd>()

                                            // Get Everything
                                            InfoFactUsGaapIncomeLossUnitsUsd = data.InfoFact?.InfoFactUsGaap?.InfoFactUsGaapNetIncomeLoss?
                                            .InfoFactUsGaapIncomeLossUnits?.InfoFactUsGaapIncomeLossUnitsUsd?
                                            .Where(usd => usd.Form != null && usd.Frame != null)
                                            .ToArray() ?? Array.Empty<InfoFactUsGaapIncomeLossUnitsUsd>()
                                        }
                                    }
                                }
                            }
                        };
                        companies.Add(company);
                        cikImportResult.SucceededCiks.Add(cik);
                    }
                    catch (Exception ex)
                    {
                        // Log error and continue with next CIK
                        _logger.LogError(ex, $"Failed to retrieve company information for CIK ID: {cik}");
                        cikImportResult.FailedCiks.Add(cik);
                        continue;
                    }
                    finally
                    {
                        // Increment index counter
                        currentIndex++;
                    }
                }

                // Save to database
                await Repository.CompanyInfoRepository.CreateAllAsync(companies);
            }

            // Update Public Company table with the failed CIKs
            HashSet<int> ciksHashSet = new HashSet<int>(ciks.Select(cik => int.Parse(cik)));
            List<PublicCompany> publicCompanies = await Repository.PublicCompanyRepository.GetByConditionAsync(pc => ciksHashSet.Contains(pc.Cik));

            HashSet<string> failedCiks = [.. cikImportResult.FailedCiks];

            publicCompanies.ForEach(pc =>
            {
                // Update Market Data Loaded status for this Security
                if (failedCiks.Contains(pc.Cik.ToPaddedCik()))
                {
                    pc.IsMarketDataLoaded = false;
                }
                else
                {
                    pc.IsMarketDataLoaded = true;
                }

                // Update Last Updated date
                pc.LastUpdated = DateTime.UtcNow;
            });

            // Bulk update all public companies
            await Repository.PublicCompanyRepository.BulkUpdateAllAsync(publicCompanies);

            // If some failed, we can set a different message
            if (cikImportResult.FailedCiks.Any())
            {
                cikImportResult.Message = "Import completed with some failures.";
            }
            else
            {
                cikImportResult.Message = "All CIKs imported successfully.";
            }

            return cikImportResult;
        }

        // Get Fundable Companies
        public async Task<List<FundableCompanyDto>> GetCompaniesAsync(string? startsWith = null)
        {
            var fundableCompanies = new List<FundableCompanyDto>();

            var companies = await Repository.CompanyInfoRepository.GetCompaniesWithDetails(startsWith);

            if (companies.Any())
            {
                foreach (var company in companies)
                {
                    var incomeData = company?.InfoFact?.InfoFactUsGaap?.InfoFactUsGaapNetIncomeLoss?.InfoFactUsGaapIncomeLossUnits?.InfoFactUsGaapIncomeLossUnitsUsd;
                    var incomeDataList = incomeData ?? new List<InfoFactUsGaapIncomeLossUnitsUsd>();

                    var standardAmount = this.CalculateStandardFundableAmount(incomeDataList);

                    var income2021 = incomeDataList.FirstOrDefault(d => d.Frame == FrameConst.Year2021)?.Val ?? (int)FundableAmountEnum.StandardFundableAmount;
                    var income2022 = incomeDataList.FirstOrDefault(d => d.Frame == FrameConst.Year2022)?.Val ?? (int)FundableAmountEnum.StandardFundableAmount;

                    var specialAmount = this.CalculateSpecialFundableAmount(standardAmount, company?.EntityName ?? "", income2021, income2022);

                    fundableCompanies.Add(new FundableCompanyDto
                    {
                        Id = company.Cik,
                        Name = company.EntityName,
                        StandardFundableAmount = standardAmount,
                        SpecialFundableAmount = specialAmount
                    });
                }
            }

            return fundableCompanies.OrderBy(c => c.Name).ToList();
        }

        // Calculate Standard Fundable Amount
        public decimal CalculateStandardFundableAmount(IEnumerable<InfoFactUsGaapIncomeLossUnitsUsd> incomeData)
        {
            try
            {
                // Filter for yearly data only
                var yearlyData = incomeData
                    .Where(usd => TargetYears.Contains(GetYearFromFrame(usd.Frame)))
                    .GroupBy(usd => GetYearFromFrame(usd.Frame)) // only one entry per year
                    .Select(g => g.FirstOrDefault()) // pick the first entry for the year
                    .ToList();

                // Check if we have data for all required years
                if (yearlyData.Count != TargetYears.Count)
                {
                    // Return Standard Fundable Amount
                    return (int)FundableAmountEnum.StandardFundableAmount;
                }
                ;

                // Check if income is positive for specific years
                if (!yearlyData.Any(d => d.Frame.StartsWith(FrameConst.Year2021) && d.Val > (int)IncomeEnum.PositiveIncome) ||
                    !yearlyData.Any(d => d.Frame.StartsWith(FrameConst.Year2022) && d.Val > (int)IncomeEnum.PositiveIncome))
                {
                    return (int)FundableAmountEnum.StandardFundableAmount;
                }

                // Calculate highest income
                var highestIncome = yearlyData.Max(d => d.Val);

                // Apply different rates based on income threshold
                decimal rate = highestIncome >= FundingRateConst.TenBillionThreshold ?
                    FundingRateConst.LargeIncomeMultiplier :
                    FundingRateConst.SmallerIncomeMultiplier;

                return highestIncome * rate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to calculate standard fundable amount.");
                return 0;
            }
        }

        // Helper method to extract the year from the frame
        public int GetYearFromFrame(string frame)
        {
            // Extract numeric year from frame
            var match = System.Text.RegularExpressions.Regex.Match(frame, @"CY(\d{4})");
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }

        // Calculate Special Fundable Amount
        public decimal CalculateSpecialFundableAmount(decimal standardAmount, string name, decimal income2021, decimal income2022)
        {
            var specialAmount = standardAmount;

            // Check if the name is not null or empty, and starts with a vowel
            if (!string.IsNullOrEmpty(name) && CompanyNameConst.Vowels.Contains(char.ToUpper(name[0])))
            {
                specialAmount += standardAmount * FundingRateConst.CompanyNameWithVowelMultiplier;
            }

            // Check if the 2022 income is less than the 2021 income
            if (income2022 < income2021)
            {
                specialAmount += standardAmount * FundingRateConst.CompanyNameWithDecreasingIncomeMultiplier;
            }

            return specialAmount;
        }

        // Create Market Data Load Record
        public async Task CreateMarketDataLoadRecord()
        {
            _logger.LogInformation("Create Market Data Load.");

            // Create Market Data Load Status in the database
            await Repository.Context.AddAsync(new MarketDataLoadRecord
            {
                LoadDate = DateTime.UtcNow
            });

            // Save changes to the database
            await Repository.Context.SaveChangesAsync();
        }

        // Check if Market Data is loaded
        public async Task<bool> IsMarketDataLoadedAsync()
        {
            return await Repository.Context.MarketDataLoadRecord.AnyAsync();
        }
    }
}
