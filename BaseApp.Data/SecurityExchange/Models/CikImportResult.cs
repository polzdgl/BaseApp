using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class CikImportResult
    {
        public List<string> SucceededCiks { get; set; } = new();
        public List<string> FailedCiks { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
