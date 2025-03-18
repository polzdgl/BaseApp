using BaseApp.Data.Company.Interfaces;
using BaseApp.Data.Company.Models;
using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Shared.Enums.Compnay;
using Microsoft.EntityFrameworkCore;
using BaseApp.Shared.Validations;

namespace BaseApp.Data.Company.Repository
{
    public class CompanyInfoRepository : GenericRepository<CompanyInfo>, ICompanyInfoRepository
    {
        public CompanyInfoRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        // Get all CIKs that need to be imported, and format them to be 10 characters long
        public async Task<IEnumerable<string>> GetAllCikIds()
        {
            return await GetAll().Select(x => x.Cik.ToPaddedCik()).ToListAsync();
        }

        // Get all companies with details by loading navigation properties
        public async Task<IEnumerable<CompanyInfo>> GetCompaniesWithDetails(string? startsWith = null)
        {
            var query = startsWith == null
                ? GetAll()
                : GetByCondition(x => x.EntityName.StartsWith(startsWith));

            return await LoadCompanyDetails(query);
        }

        public async Task<CompanyInfo?> GetCompanyWithDetailsByCik(int cik)
        {
            var query = GetByCondition(x => x.Cik == cik);

            var result = await LoadCompanyDetails(query);

            return result.FirstOrDefault();
        }

        // Load navigation properties for CompanyInfo, takes query as parap with filter
        private async Task<IEnumerable<CompanyInfo>> LoadCompanyDetails(IQueryable<CompanyInfo> query)
        {
            return await query
                .Include(x => x.InfoFact)
                .ThenInclude(x => x.InfoFactUsGaap)
                .ThenInclude(x => x.InfoFactUsGaapNetIncomeLoss)
                .ThenInclude(x => x.InfoFactUsGaapIncomeLossUnits)
                .ThenInclude(x => x.InfoFactUsGaapIncomeLossUnitsUsd)
                .ToListAsync();
        }

        // Get CIKs that need to be imported by comparing CIKs in PublicCompany and CompanyInfo
        public async Task<IEnumerable<string>> GetCiksToImportAsync()
        {
            // Query PublicCompany CIKs sequentially.
            var publicCiks = await _dbContext.PublicCompany
                .Select(pc => pc.Cik.ToPaddedCik())
                .ToListAsync();

            // Then query CompanyInfo CIKs sequentially.
            var importedCiks = await _dbContext.CompanyInfo
                .Select(ci => ci.Cik.ToPaddedCik())
                .ToListAsync();

            // Return all CIKs present in PublicCompany but not in CompanyInfo.
            return publicCiks.Except(importedCiks);
        }
    }
}
