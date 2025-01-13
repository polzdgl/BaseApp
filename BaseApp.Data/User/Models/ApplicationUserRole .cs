using Microsoft.AspNetCore.Identity;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public DateTime DateAssigned { get; set; }
        public string? AssignedBy { get; set; }
    }
}
