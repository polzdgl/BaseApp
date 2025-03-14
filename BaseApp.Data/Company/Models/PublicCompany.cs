using System.Text.Json.Serialization;

namespace BaseApp.Data.Company.Models
{
    public class PublicCompany
    {
        public int Id { get; set; }

        private string _name { get; set; }
        private string _ticker { get; set; }

        [JsonPropertyName("title")]
        public string Name
        {
            get => _name;
            set => _name = value.ToLowerInvariant();
        }

        [JsonPropertyName("ticker")]
        public string Ticker { 
            get => _ticker; 
            set => _ticker = value.ToUpperInvariant(); 
        }

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
    }
}
