using System;

namespace Youpay.API.Utils
{
    public interface IUserUtil
    {
        string GenerateRandomPassword(int length);
        string GenerateRandomId(int length);
        string GenerateTransactionCode(String previousCode);
    }
}