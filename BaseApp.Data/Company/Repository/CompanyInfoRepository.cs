using BaseApp.Data.Company.Interfaces;
using BaseApp.Data.Company.Models;
using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Shared.Enums.Compnay;
using Microsoft.EntityFrameworkCore;

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
            return await GetAll().Select(x => x.Cik.ToString().Trim()
            .PadLeft((int)CikPaddingEnum.PaddingNumber, (char)CikPaddingEnum.PaddingValue)).ToListAsync();
        }

        // Get all companies with details by loading navigation properties
        public async Task<IEnumerable<CompanyInfo>> GetCompaniesWithDetails(int page, int pageSize, string? startsWith = null)
        {
            var query = startsWith == null
                ? GetPagedQueryable(page, pageSize)
                : GetByCondition(x => x.EntityName.StartsWith(startsWith));

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
                .Select(pc => pc.Cik.ToString().Trim().PadLeft((int)CikPaddingEnum.PaddingNumber, '0'))
                .ToListAsync();

            // Then query CompanyInfo CIKs sequentially.
            var importedCiks = await _dbContext.CompanyInfo
                .Select(ci => ci.Cik.ToString().Trim().PadLeft((int)CikPaddingEnum.PaddingNumber, '0'))
                .ToListAsync();

            // Return all CIKs present in PublicCompany but not in CompanyInfo.
            return publicCiks.Except(importedCiks);
        }
    }
}
