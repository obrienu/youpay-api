using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using Youpay.API.Helpers;
using Youpay.API.Models;

namespace Youpay.API.Services.Impl
{
    public class MailGunMailingService : IMailingServices
    {
        private readonly IConfiguration _config;
        public MailGunMailingService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IRestResponse> SendMessage(string message, string email,string subject)
        {
            var DOMAIN_NAME = _config.GetSection("MailGunSetting:DomainName").Value;
            var BASE_URL = _config.GetSection("MailGunSetting:BaseUrl").Value;
            var API_KEY = _config.GetSection("MailGunSetting:ApiKey").Value;

            RestClient client = new RestClient();
            client.BaseUrl = new Uri(BASE_URL);
            client.Authenticator = new HttpBasicAuthenticator("api",API_KEY);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", DOMAIN_NAME, ParameterType.UrlSegment);
            request.Resource = DOMAIN_NAME + "/messages";
            request.AddParameter("from", "obrienu@live.com");
            request.AddParameter("to", "obrienules@gmail.com");
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);
            request.Method = Method.POST;
            return await client.ExecuteAsync(request);
        }

        public async Task<bool> SendPassword(string name, string email, string password)
        {
            string message = String.Format(@"Hello {0}, welcome to Youpay.
            Your login credentials are your email and the password provided below.
            email: {1}
            password: {2}.
            Login to your account and reset the password.
            ", name, email, password);

            var subject = "Welcome to YouPay";

            var response = await SendMessage(message, email, subject);
            int count = 0;
            while(count < 2 && !response.IsSuccessful)
            {
                response =  await SendMessage(message, email, subject);
                count++;
            }
            return response.IsSuccessful;
        }

        public async Task<bool> SendPasswordResetToken(string name, string token, string email)
        {
            string message = String.Format(@"Hello {0}.
            Your requested to reset your password is being processed, enter the token below to complete the process.
            token: {1}.
            Note that token will expire in 10 minutes.
            ", name, token);

            var subject = "Welcome to YouPay";

            var response = await SendMessage(message, email, subject);
            int count = 0;
            while(count < 2 && !response.IsSuccessful)
            {
                response =  await SendMessage(message, email, subject);
                count++;
            }
            return response.IsSuccessful;
        }

        public async Task<bool> SendTransactionMail(User mainRecipient, User otherRecipient)
        {
            string message = String.Format(@"Hello {0}.
            The transaction with {1} has been registered.
            Please notify {2} to check {2} email for the transaction details and link.
            ", mainRecipient.Firstname + " " + mainRecipient.LastName,
             otherRecipient.Firstname + " " + otherRecipient.LastName
             , otherRecipient.Sex.Equals(Gender.MALE) ? "he": "her");

            var subject = "Registration of Transaction";

            var response = await SendMessage(message, mainRecipient.Email, subject);
            int count = 0;
            while(count < 2 && !response.IsSuccessful)
            {
                response =  await SendMessage(message, mainRecipient.Email, subject);
                count++;
            }
            return response.IsSuccessful;
        }
        public async Task<bool> sendNotificationOfPayment(Transaction transaction)
        {
            string message = String.Format(@"Hello {0}.
            {1} has made payment for this transaction .
            You can proceed with processing and making dilivery.
            ", transaction.Merchant.Firstname + " " + transaction.Merchant.LastName,
             transaction.Buyer.Firstname + " " + transaction.Buyer.LastName );

            var subject = "Notification of Payment";

            var response = await SendMessage(message, transaction.Merchant.Email, subject);
            int count = 0;
            while(count < 2 && !response.IsSuccessful)
            {
                response =  await SendMessage(message, transaction.Merchant.Email, subject);
                count++;
            }
            return response.IsSuccessful;
        }

        public async Task<bool> sendNotificationOfShippment(Transaction transaction)
        {
            string message = String.Format(@"Hello {1}.
            {0} has shipped your order - {2}.
            Please kindly verify delivery stauts on recieving your order.
            ", transaction.Merchant.Firstname + " " + transaction.Merchant.LastName,
             transaction.Buyer.Firstname + " " + transaction.Buyer.LastName,
             transaction.ProductName );

            var subject = "Notification of Shipment";

            var response = await SendMessage(message, transaction.Buyer.Email, subject);
            int count = 0;
            while(count < 2 && !response.IsSuccessful)
            {
                response =  await SendMessage(message, transaction.Buyer.Email, subject);
                count++;
            }
            return response.IsSuccessful;
        }
        public async Task<bool> sendNotificationOfDelivery(Transaction transaction)
        {
            string message = String.Format(@"Hello {0}.
            {1} has recieved his/her order - {2}.
            Payment will be made to your main bank account on or less than 48 hours after confirmation.
            Note that payment will be Transaction amount less the charges - #{3}
            ", transaction.Merchant.Firstname + " " + transaction.Merchant.LastName,
             transaction.Buyer.Firstname + " " + transaction.Buyer.LastName,
             transaction.ProductName,
              transaction.Charges - 500 );

            var subject = "Notification of Dilivery";

            var response = await SendMessage(message, transaction.Buyer.Email, subject);
            int count = 0;
            while(count < 2 && !response.IsSuccessful)
            {
                response =  await SendMessage(message, transaction.Buyer.Email, subject);
                count++;
            }
            return response.IsSuccessful;
        }
    }
}