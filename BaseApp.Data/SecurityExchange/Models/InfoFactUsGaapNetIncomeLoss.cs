using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo;
using System.Text.Json.Serialization;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFactUsGaapNetIncomeLoss
    {
        public int Id { get; set; }

        public int InfoFactUsGaapId { get; set; }

        [JsonPropertyName("units")]
        public InfoFactUsGaapIncomeLossUnits Units { get; set; }
    }
}
