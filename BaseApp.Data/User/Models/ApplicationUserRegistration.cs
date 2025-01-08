using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserRegistration
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
