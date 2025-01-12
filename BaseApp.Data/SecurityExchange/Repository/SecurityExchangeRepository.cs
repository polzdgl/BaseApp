
using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Data.SecurityExchange.Interfaces;
using BaseApp.Data.SecurityExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.SecurityExchange.Repository
{
    public class SecurityExchangeRepository : GenericRepository<EdgarCompanyInfo>, ISecurityExchangeRepository
    {
        public SecurityExchangeRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<EdgarCompanyInfo>> GetCompaniesWithDetails(string? startsWith = null)
        {
            var query = startsWith == null
                ? this.GetAll()
                : this.GetByCondition(x => x.EntityName.StartsWith(startsWith));

            return await query
                .Include(x => x.InfoFact)
                .ThenInclude(x => x.InfoFactUsGaap)
                .ThenInclude(x => x.InfoFactUsGaapNetIncomeLoss)
                .ThenInclude(x => x.InfoFactUsGaapIncomeLossUnits)
                .ThenInclude(x => x.InfoFactUsGaapIncomeLossUnitsUsd)
                .ToListAsync();
        }
    }
}
