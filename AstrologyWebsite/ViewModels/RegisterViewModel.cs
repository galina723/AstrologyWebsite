using System.ComponentModel.DataAnnotations;

namespace AstrologyWebsite.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare ("Password", ErrorMessage="Password don't match")]
        public string? ConfirmPassword { get; set; }

    }
}
