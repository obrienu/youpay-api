using System;

namespace Youpay.API.Utils.Impl
{
    public class TransactionsUtil : ITransactionsUtil
    {
        public string GenerateRandomValues(int length)
        {
             Random random = new Random();
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var lengthOfCharacters = characters.Length;
            var answer = "";
            for (int i = 0; i < length; i++)
            {
                answer += characters[random.Next(lengthOfCharacters)];
            }
            return answer;
        }

        public string GenerateTransactionCode(String previousCode) 
        {
            if(previousCode == null){
                return "YOU0000001";
            }

            var stringDigit  = previousCode.Substring(3);
            var nextDigit = (Int64.Parse(stringDigit) + 1).ToString().PadLeft(7, '0');
            return "YOU" + nextDigit ;
        }
    }

    
}