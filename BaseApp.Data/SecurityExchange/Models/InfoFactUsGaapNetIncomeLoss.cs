using System.Text.Json.Serialization;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFactUsGaapNetIncomeLoss
    {
        public int Id { get; set; }

        public int InfoFactUsGaapId { get; set; }

        [JsonPropertyName("units")]
        public virtual InfoFactUsGaapIncomeLossUnits InfoFactUsGaapIncomeLossUnits { get; set; }
    }
}
