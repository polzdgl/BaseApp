using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.User.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string? Source { get; set; }
        public DateTime VerifiedDate { get; set; }
    }
}
