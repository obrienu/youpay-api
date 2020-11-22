namespace Youpay.API.Dtos
{
    public class UserDtoForTransaction
    {
        public long? Id { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}