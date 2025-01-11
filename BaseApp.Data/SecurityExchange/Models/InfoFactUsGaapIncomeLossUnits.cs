using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFactUsGaapIncomeLossUnits
    {
        public int Id { get; set; }

        public int InfoFactUsGaapNetIncomeLossId { get; set; }

        [JsonPropertyName("USD")]
        public ICollection<InfoFactUsGaapIncomeLossUnitsUsd> Usd { get; set; }
    }
}
