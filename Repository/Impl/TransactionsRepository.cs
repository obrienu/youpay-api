using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Youpay.API.Data;
using Youpay.API.Helpers;
using Youpay.API.Dtos;
using Youpay.API.Models;

namespace Youpay.API.Repository.Impl
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly DataContext _context;
        public TransactionsRepository(DataContext context)
        {
            _context = context;
        }
        public async void AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public  void DeleteTransaction(Transaction transaction)
        {
             _context.Transactions.Remove(transaction);
        }

        public async Task<Transaction> FindTransactionById(long id)
        {
            return await _context.Transactions
            .Include(trans => trans.Buyer)
            .Include(trans => trans.Merchant)
            .FirstOrDefaultAsync(trans => trans.Id.Equals(id));
        }

        public async Task<PagedList<Transaction>> FindUsersTransaction(UserTransactionsParams userTransactionsParams, long userId)
        {
            IQueryable<Transaction> transactions = _context.Transactions
                .Include(trans => trans.Merchant)
                .Include(trans => trans.Buyer);

            switch (userTransactionsParams.Completed)
            {
                case "true":
                   
                        transactions
                         .Where(tran => tran.Buyer.Id == userId 
                                    || tran.Merchant.Id == userId && tran.Completed == true)
                        .Include(tran => tran.Buyer) 
                        .Include(tran => tran.Merchant);
                    break;
                default:
                        transactions
                        .Where(tran => tran.Buyer.Id == userId 
                                    || tran.Merchant.Id == userId && tran.Completed == false)
                        .Include(tran => tran.Buyer) 
                        .Include(tran => tran.Merchant);
                        
                    break;
            }

            if(userTransactionsParams.OrderDirection == "asc")
            {
                transactions.OrderBy( trans => trans.CreatedAt);
            }
            else
            {
                transactions.OrderByDescending( trans => trans.CreatedAt);
            }

            return await PagedList<Transaction>.CreateAsync(transactions, 
                        userTransactionsParams.PageNumber, userTransactionsParams.PageSize);
        
        }

        public async Task<bool> SaveChanges()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TransactionExists(long id)
        {
            return await _context.Transactions.AnyAsync(trans => trans.Id.Equals(id));
        }

        public  void UpdateTransaction(Transaction transaction)
        {
             _context.Transactions.Update(transaction);
        }

        public async Task<string> GetLastGeneratedCode(){
           var transaction = await _context.Transactions.OrderByDescending(trans => trans.CreatedAt).FirstOrDefaultAsync();
           if(transaction == null)
            {
                return "";
            }
           return transaction.Code;
        }
    }
}