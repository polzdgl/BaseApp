using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFactUsGaap
    {
        public int Id { get; set; }

        public int InfoFactId { get; set; }

        [JsonPropertyName("NetIncomeLoss")]
        public InfoFactUsGaapNetIncomeLoss NetIncomeLoss { get; set; }
    }
}
