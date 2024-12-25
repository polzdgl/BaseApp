using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public DateTime DateAssigned { get; set; }
        public string? AssignedBy { get; set; }
    }
}
