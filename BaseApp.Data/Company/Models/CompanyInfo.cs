using System.Text.Json.Serialization;

namespace BaseApp.Data.Company.Models
{
    public class CompanyInfo
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        [JsonPropertyName("cik")]
        public int Cik { get; set; }

        public int PublicCompanyId { get; set; }

        [JsonPropertyName("entityName")]
        public string EntityName { get; set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

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

        [JsonPropertyName("facts")]
        public virtual InfoFact InfoFact { get; set; }
    }
}
