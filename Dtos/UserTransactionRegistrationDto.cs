using System.ComponentModel.DataAnnotations;

namespace Youpay.API.Dtos
{
    public class UserTransactionRegistrationDto
    {
        [Required, StringLength(100)]
        public string ProductName { get; set; }
        [Required, StringLength(100)]
        public string Category { get; set; }
        [Required]
        public int Charges { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }
        public string SecondPartyAs { get; set; }
        public UserRegistrationDto SecondParty { get; set; }
        public BankingDetailsRegistrationDto SecondPartyBankingDetails { get; set; }

    }
}