using System.ComponentModel.DataAnnotations;
using Youpay.API.Helpers;

namespace Youpay.API.Dtos
{
    public class UserRegistrationDto
    {
        [Required, MaxLength(20)]
        public string Firstname { get; set; }

        [Required, MaxLength(20)]
        public string LastName { get; set; }
 
        [Required, MaxLength(6)]
        public string Sex { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
  
        [Required, MinLength(11, ErrorMessage="Mobile Number cannot be less than 11 digit")]
        public string PhoneNumber { get; set; }
               
    }
}