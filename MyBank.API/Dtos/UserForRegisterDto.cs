using System.ComponentModel.DataAnnotations;

namespace MyBank.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; } 

        [Required]
        [EmailAddress]  
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 7, ErrorMessage = "Password must be between 7 and 16 characters long")]
        public string Password { get; set; }
    }
}