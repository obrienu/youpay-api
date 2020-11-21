using Youpay.API.Helpers;

namespace Youpay.API.Dtos
{
    public class BankingDetailsDto
    {
        public long? Id { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public bool IsMain { get; set; }
    }
}