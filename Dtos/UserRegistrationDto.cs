using System.ComponentModel.DataAnnotations;
using Youpay.API.Helpers;

namespace Youpay.API.Dtos
{
    public class UserRegistrationDto
    {
        [MaxLength(20)]
        public string Firstname { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }
 
        [MaxLength(6)]
        public string Sex { get; set; }

        [EmailAddress]
        public string Email { get; set; }
  
        [MinLength(11, ErrorMessage="Mobile Number cannot be less than 11 digit")]
        public string PhoneNumber { get; set; }
               
    }
}