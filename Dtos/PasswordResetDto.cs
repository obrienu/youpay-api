using System.ComponentModel.DataAnnotations;

namespace Youpay.API.Dtos
{
    public class PasswordResetDto
    {
        [Required, StringLength(8, ErrorMessage="Token must be 8 characters")]
        public string Token { get; set; }
        [DataType(DataType.Password)]
        [Required, RegularExpression("^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\\W).*$",
         ErrorMessage="Password must be at least 8 characters long, contain at least one uppercase, lowercase special character and digit")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Must be equal to Password")]
        public string ConfirmPassword { get; set; }

    }
}