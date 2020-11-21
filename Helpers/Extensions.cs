using Microsoft.AspNetCore.Http;

namespace Youpay.API.Helpers
{
    public static class Extensions
    {
         public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

        }

        public static Gender SetGender(this string gender)
        {
            switch (gender.ToLower())
            {
                case "male":
                    return Gender.MALE;
                default:
                    return Gender.FEMALE;
            }

        }

        public static string SetGenderFromEnum(this Gender gender)
        {
            switch (gender)
            {
                case Gender.MALE:
                    return "Male";
                default:
                    return "Female";
            }
        }

        public static AccountType SetAccountType(this string accountType)
        {
            switch (accountType.ToLower())
            {
                case "savings":
                    return AccountType.SAVINGS;
                case "current":
                    return AccountType.CURRENT;
                default:
                    return AccountType.CREDIT;
            }

        }
        public static string SetAccountTypeFromEnum(this AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.SAVINGS:
                    return "Savings";
                case AccountType.CURRENT:
                    return "Current";
                default:
                    return "Credit";
            }

        }
    }
}