using System.Collections.Generic;
using System.Threading.Tasks;
using Youpay.API.Dtos;
using Youpay.API.Models;

namespace Youpay.API.Repository
{
    public interface IBankingDetailsRepository
    {
        Task<BankingDetails> FindByAccountNumber(long accountNumber);
        Task<BankingDetails> FindById(long id);
        void AddBankingDetails(BankingDetails bankingDetails);
        void UpdateBankingDetails(BankingDetails bankingDetails);
        void DeleteBankingDetails(BankingDetails bankingDetails);
        Task<bool> SaveChanges();
    }
}