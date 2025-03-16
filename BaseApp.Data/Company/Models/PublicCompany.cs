using System.Text.Json.Serialization;

namespace BaseApp.Data.Company.Models
{
    public class PublicCompany
    {
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("cik_str")]
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

        // Flag to check if the market data is loaded for this Securiry
        public bool? IsMarketDataLoaded { get; set; }

        // Date when the market data was last updated
        public DateTime? LastUpdated { get; set; }
    }
}
