using System;

namespace Youpay.API.Dtos
{
    public class LoginDto
    {
        public String Token { get; set; }
        public long? UserId { get; set; }
        public String Role { get; set; }
        public String Email { get; set; }
    }
}