using System.Threading.Tasks;
using Youpay.API.Models;

namespace Youpay.API.Services
{
    public interface IMailingServices
    {
        Task<bool> SendPassword(string name, string email, string password);
        Task<bool> SendPasswordResetToken(string name, string token, string email);
        Task<bool> SendTransactionMail(User mainRecipient, User otherRecipient);
        Task<bool> sendNotificationOfPayment(Transaction transaction);
        Task<bool> sendNotificationOfShippment(Transaction transaction);
        Task<bool> sendNotificationOfDelivery(Transaction transaction);
    }
}