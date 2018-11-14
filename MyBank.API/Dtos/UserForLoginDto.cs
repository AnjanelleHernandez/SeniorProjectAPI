using System.ComponentModel.DataAnnotations;

namespace MyBank.API.Dtos
{
    public class UserForLoginDto
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}