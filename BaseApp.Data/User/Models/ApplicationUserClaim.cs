using Microsoft.AspNetCore.Identity;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public string? Source { get; set; }
        public DateTime VerifiedDate { get; set; }
    }
}
