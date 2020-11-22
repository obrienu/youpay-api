using System.ComponentModel.DataAnnotations;

namespace Youpay.API.Dtos
{
    public class FirstTimeUserTransactionRegisterationDto
    {
        [Required, StringLength(100)]
        public string ProductName { get; set; }
        [Required, StringLength(100)]
        public string Category { get; set; }
        [Required]
        public int Charges { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }
        public UserRegistrationDto Merchant { get; set; }
        public BankingDetailsRegistrationDto MerchantBankingDetails { get; set; }
        public UserRegistrationDto Buyer { get; set; }
        public BankingDetailsRegistrationDto BuyerBankingDetails { get; set; }

    }
}