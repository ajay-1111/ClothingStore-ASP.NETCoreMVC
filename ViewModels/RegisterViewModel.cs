using System.ComponentModel.DataAnnotations;

namespace Clothing_Store.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name field is mandatory.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name field is mandatory.")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Telephone number is mandatory.")]
        [StringLength(100, MinimumLength = 10)]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Email Address is mandatory.")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is mandatory.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}