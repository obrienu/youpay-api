using System.ComponentModel.DataAnnotations;

namespace Youpay.API.Dtos
{
    public class UserLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}