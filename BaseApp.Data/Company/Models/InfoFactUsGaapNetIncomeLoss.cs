using System.Text.Json.Serialization;

namespace BaseApp.Data.Company.Models
{
    public class InfoFactUsGaapNetIncomeLoss
    {
        public int Id { get; set; }

        public int InfoFactUsGaapId { get; set; }

        [JsonPropertyName("label")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("units")]
        public virtual InfoFactUsGaapIncomeLossUnits InfoFactUsGaapIncomeLossUnits { get; set; }
    }
}
