﻿using System.Text.Json.Serialization;

namespace BaseApp.Data.Company.Models
{
    public class InfoFactUsGaap
    {
        public int Id { get; set; }

        public int InfoFactId { get; set; }

        [JsonPropertyName("NetIncomeLoss")]
        public virtual InfoFactUsGaapNetIncomeLoss InfoFactUsGaapNetIncomeLoss { get; set; }
    }
}
