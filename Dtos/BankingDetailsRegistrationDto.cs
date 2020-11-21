using System.ComponentModel.DataAnnotations;

namespace Youpay.API.Dtos
{
    public class BankingDetailsRegistrationDto
    {
        [Required, MinLength(3, ErrorMessage="Bank name cannot be less than 3 characters")]
        [MaxLength(50)]
        public string BankName { get; set; }
        [Required, MinLength(3, ErrorMessage="Account name cannot be less than 3 characters")]
        [MaxLength(100)]
        public string AccountName { get; set; }
        [Required, RegularExpression("\\d{10,}", ErrorMessage="Account number can't be less than 10 digits long")]
        public string AccountNumber { get; set; }
        [Required, RegularExpression("current|savings|credit", ErrorMessage="Account type can either be credit, savings or current")]
        public string AccountType { get; set; }
        [Required]
        public bool IsMain { get; set; }
        
    }
}