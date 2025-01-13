using Microsoft.AspNetCore.Identity;

namespace BaseApp.Data.User.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
        public string? Permissions { get; set; }
    }
}
