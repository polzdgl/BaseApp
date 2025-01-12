using System.Text.Json.Serialization;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class EdgarCompanyInfo
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        [JsonPropertyName("cik")]
        public int Cik { get; set; }

        [JsonPropertyName("entityName")]
        public string EntityName { get; set; }

        [JsonPropertyName("facts")]
        public virtual InfoFact InfoFact { get; set; }
    }
}
