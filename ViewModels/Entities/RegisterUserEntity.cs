using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Clothing_Store.ViewModels.Entities
{
    public class RegisterUserEntity : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Password { get; set; }
    }
}