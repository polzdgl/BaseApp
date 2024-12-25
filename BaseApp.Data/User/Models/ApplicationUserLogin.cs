using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public string? LoginProviderDetails { get; set; }
    }
}
