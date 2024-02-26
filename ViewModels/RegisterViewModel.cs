using System.ComponentModel.DataAnnotations;

namespace Clothing_Store.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "First Name feild is mandatory.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Last Name feild is mandatory.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Telephone number is mandatory.", MinimumLength = 10)]
        public string Telephone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email Address is manadatory.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
