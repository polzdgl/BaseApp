using Microsoft.AspNetCore.Identity;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public DateTime ExpirationDate { get; set; }
    }
}
