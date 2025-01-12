using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo;

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
