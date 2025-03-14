using System.Text.Json.Serialization;

namespace BaseApp.Data.Company.Models
{
    public class InfoFactUsGaapIncomeLossUnitsUsd
    {
        public int Id { get; set; }

        public int InfoFactUsGaapIncomeLossUnitsId { get; set; }

        [JsonPropertyName("start")]
        public DateOnly StartDate { get; set; }

        [JsonPropertyName("end")]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// The income/loss amount.
        /// </summary>
        [JsonPropertyName("val")]
        public decimal Val { get; set; }

        [JsonPropertyName("fy")]
        public int FiscalYear { get; set; }

        [JsonPropertyName("fp")]
        public string FiscalPeriod { get; set; }

        [JsonPropertyName("filed")]
        public DateOnly FiledAt { get; set; }

        /// <summary>
        /// Possibilities include 10-Q, 10-K,8-K, 20-F, 40-F, 6-K, and
        /// their variants.YOU ARE INTERESTED ONLY IN 10-K DATA!
        /// </summary>
        [JsonPropertyName("form")]
        public string Form { get; set; }

        /// <summary>
        /// For yearly information, the format is CY followed by the year
        /// number.For example: CY2021.YOU ARE INTERESTED ONLY IN YEARLY INFORMATION
        /// WHICH FOLLOWS THIS FORMAT!
        /// </summary>
        [JsonPropertyName("frame")]
        public string Frame { get; set; }

    
    }
}
