using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseApp.Data.Company.Dtos
{
    public class CompanyFinancialsDto
    {
        public int InfoFactUsGaapIncomeLossUnitsUsdId { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal Value { get; set; }
        public int? FiscalYear { get; set; }
        public string? FiscalPeriod { get; set; }
        public DateOnly? FiledAt { get; set; }

        /// <summary>
        /// Possibilities include 10-Q, 10-K,8-K, 20-F, 40-F, 6-K, and
        /// their variants.YOU ARE INTERESTED ONLY IN 10-K DATA!
        /// </summary>
        public string Form { get; set; }

        /// <summary>
        /// For yearly information, the format is CY followed by the year
        /// number.For example: CY2021.YOU ARE INTERESTED ONLY IN YEARLY INFORMATION
        /// WHICH FOLLOWS THIS FORMAT!
        /// </summary>
        public string Frame { get; set; }
    }
}
