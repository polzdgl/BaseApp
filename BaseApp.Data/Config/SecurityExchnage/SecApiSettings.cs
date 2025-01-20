using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.Config.SecurityExchnage
{
    public class SecApiSettings
    {
        public string BaseUrl { get; set; }
        public string EdgarCompanyInfoUrl { get; set; }
        public string EdgarCompanyFilingsUrl { get; set; }
        public string EdgarCompanyFilingDetailUrl { get; set; }
        public string EdgarCompanyFilingDetailHtmlUrl { get; set; }
        public string CompanyTickers { get; set; }
    }
}