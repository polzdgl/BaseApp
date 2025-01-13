using Microsoft.AspNetCore.Identity;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public string? LoginProviderDetails { get; set; }
    }
}
