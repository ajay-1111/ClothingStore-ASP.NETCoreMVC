using Microsoft.AspNetCore.Identity;

namespace Clothing_Store.ViewModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
