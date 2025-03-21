﻿using BaseApp.Data.Company.Interfaces;
using BaseApp.Data.Company.Models;
using BaseApp.Data.Context;
using BaseApp.Data.Repositories;
using BaseApp.Shared.Enums.Compnay;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Company.Repository
{
    public class EdgarCompanyInfoRepository : GenericRepository<EdgarCompanyInfo>, IEdgarCompanyInfoRepository
    {
        public EdgarCompanyInfoRepository(ApplicationDbContext context)
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
        public async Task<IEnumerable<EdgarCompanyInfo>> GetCompaniesWithDetails(string? startsWith = null)
        {
            var query = startsWith == null
                ? GetAll()
                : GetByCondition(x => x.EntityName.StartsWith(startsWith));

            return await query
                .Include(x => x.InfoFact)
                .ThenInclude(x => x.InfoFactUsGaap)
                .ThenInclude(x => x.InfoFactUsGaapNetIncomeLoss)
                .ThenInclude(x => x.InfoFactUsGaapIncomeLossUnits)
                .ThenInclude(x => x.InfoFactUsGaapIncomeLossUnitsUsd)
                .ToListAsync();
        }

        // Static data for CIKs that need to be imported
        // Ideally, this should be fetched from a database or a file
        public IEnumerable<string> GetCiksToImport()
        {
            return new[]
            {
                "18926", "892553", "1510524", "1858912", "1828248", "1819493", "60086",
                "1853630", "1761312", "1851182", "1034665", "927628", "1125259", "1547660",
                "1393311", "1757143", "1958217", "312070", "310522", "1861841", "1037868",
                "1696355", "1166834", "915912", "1085277", "831259", "882291", "1521036",
                "1824502", "1015647", "884624", "1501103", "1397183", "1552797", "1894630",
                "823277", "21175", "1439124", "52827", "1730773", "1867287", "1685428",
                "1007587", "92103", "1641751", "6845", "1231457", "947263", "895421",
                "1988979", "1848898", "844790", "1541309", "1858007", "1729944", "726958",
                "1691221", "730272", "1308106", "884144", "1108134", "1849058", "1435617",
                "1857518", "64803", "1912498", "1447380", "1232384", "1141788", "1549922",
                "914475", "1498382", "1400897", "314808", "1323885", "1526520", "1550695",
                "1634293", "1756708", "1540159", "1076691", "1980088", "1532346", "923796",
                "1849635", "1872292", "1227857", "1046311", "1710350", "1476150", "1844642",
                "1967078", "14272", "933267", "1157557", "1560293", "217410", "1798562",
                "1038074", "1843370"
            };
        }
    }
}
