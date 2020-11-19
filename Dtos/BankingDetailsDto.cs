using Youpay.API.Helpers;

namespace Youpay.API.Dtos
{
    public class BankingDetailsDto
    {
        public long? Id { get; set; }
        public string BankName { get; set; }
        public long AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public bool IsMain { get; set; }
    }
}