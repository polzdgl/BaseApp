using Microsoft.AspNetCore.Identity;

namespace BaseApp.Data.User.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string? Source { get; set; }
        public DateTime VerifiedDate { get; set; }
    }
}
