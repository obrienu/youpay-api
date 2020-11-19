namespace Youpay.API.Dtos
{
    public class BankingDetailsRegistrationDto
    {
        public long? Id { get; set; }
        public string BankName { get; set; }
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public bool IsMain { get; set; }
        
    }
}