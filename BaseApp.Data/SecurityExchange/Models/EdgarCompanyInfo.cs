using System.Text.Json.Serialization;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class EdgarCompanyInfo
    {
        public int Cik { get; set; }
        public string EntityName { get; set; }
        public InfoFact Facts { get; set; }
    }
}
