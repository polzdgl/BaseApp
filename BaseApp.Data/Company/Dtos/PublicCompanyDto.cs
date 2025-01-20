using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseApp.Data.Company.Dtos
{
    public class PublicCompanyDto
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("cik_str")]
        public int Cik { get; set; }
    }
}
