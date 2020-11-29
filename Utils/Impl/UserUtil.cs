using System;
using System.Collections.Generic;
using System.Text;

namespace Youpay.API.Utils.Impl
{
    public class UserUtil : IUserUtil
    {
        Random random = new Random();

        public string GenerateRandomPassword(int length)
        {
            string specialCharacters = "$#&*-_=+@";
            return GenerateRandomId(length - 1) + specialCharacters[random.Next(specialCharacters.Length)];
        }
        public string GenerateRandomId(int length)
        {
          
                if (length < 6)
                    throw new Exception("Length of Id cannot be less than 6");

                string upperCaseChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string lowerCaseChar = "abcdefghijklmnopqrstuvwxyz";
                string digit = "1234567890";

                var lengthOfEachCharType = length / 3;

                var lengthOfLowerCase = lengthOfEachCharType + length % 3;

                var answer = new List<char>();
                GenerateRandomNumber(lengthOfEachCharType, upperCaseChar, answer);
                GenerateRandomNumber(lengthOfEachCharType, digit, answer);
                GenerateRandomNumber(lengthOfLowerCase, lowerCaseChar, answer);

                return RandomiseValues(answer);
           
        }

        private void GenerateRandomNumber(int length, string charactersToUse, IList<char> listToAddThem)
        {
            var lengthOfCharacter = charactersToUse.Length;
            for (int i = 0; i < length; i++)
            {
                listToAddThem.Add(charactersToUse[random.Next(lengthOfCharacter)]);
            }
        }

        private string RandomiseValues(IList<char> listToRandomise)
        {
            var randomiseString = new StringBuilder(listToRandomise.Count);
            int index = 0;
            while (listToRandomise.Count > 0)
            {
                index = random.Next(listToRandomise.Count);
                randomiseString.Append(listToRandomise[index]);
                listToRandomise.RemoveAt(index);
            }
            return randomiseString.ToString();
        }

    }


}