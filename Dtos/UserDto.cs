using System.Collections.Generic;
using Youpay.API.Helpers;
using Youpay.API.Models;

namespace Youpay.API.Dtos
{
    public class UserDto
    {
   
        public long? Id { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public Gender Sex { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public virtual ICollection<BankingDetails> BankingDetails { get; set; }

    }
}