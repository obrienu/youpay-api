using System.ComponentModel.DataAnnotations;

namespace Youpay.API.Dtos
{
    public class RequestPasswordResetDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }   
    }
}