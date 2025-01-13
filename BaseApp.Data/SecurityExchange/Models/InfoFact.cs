using System.Text.Json.Serialization;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFact
    {
        public int Id { get; set; }

        public int EdgarCompanyInfoId { get; set; }

        [JsonPropertyName("us-gaap")]
        public virtual InfoFactUsGaap InfoFactUsGaap { get; set; }
    }
}
