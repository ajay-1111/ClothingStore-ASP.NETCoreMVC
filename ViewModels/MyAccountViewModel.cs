using System.ComponentModel.DataAnnotations;

namespace Clothing_Store.ViewModels
{
    public class MyAccountViewModel
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Telephone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string PostCode { get; set; }
    }
}
