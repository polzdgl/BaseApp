﻿using System.Text.Json.Serialization;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFactUsGaapIncomeLossUnits
    {
        public int Id { get; set; }

        public int InfoFactUsGaapNetIncomeLossId { get; set; }

        [JsonPropertyName("USD")]
        public virtual ICollection<InfoFactUsGaapIncomeLossUnitsUsd> InfoFactUsGaapIncomeLossUnitsUsd { get; set; }
    }
}
