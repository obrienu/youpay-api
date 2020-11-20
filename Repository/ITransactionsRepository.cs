using System.Collections.Generic;
using System.Threading.Tasks;
using Youpay.API.Models;
using Youpay.API.Helpers;
using Youpay.API.Dtos;

namespace Youpay.API.Repository
{
    public interface ITransactionsRepository
    {
        Task<PagedList<Transaction>>  FindUsersTransaction(UserTransactionsParams userTransactionsParams, 
         long userId);
        Task<Transaction> FindTransactionById(string id);
        void AddTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(Transaction transaction);
        Task<bool> SaveChanges();

        Task<bool> TransactionExists(string id);

    }
}