using BaseApp.Data.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseApp.Data.Company.Dtos
{
    public class CompanyDetailsDto
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public int Cik { get; set; }
        public string? Sedar { get; set; }
        public string? Isin { get; set; }
        public string? Cusip { get; set; }
        public string? Country { get; set; }
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public string? SubIndustry { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? Exchange { get; set; }
        public string? Currency { get; set; }


        // Date when the market data was last updated
        public DateTime? LastUpdated { get; set; }

        public List<CompanyFinancialsDto> CompanyFinancials { get; set; }
    }
}
