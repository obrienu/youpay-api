using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Youpay.API.Data;
using Youpay.API.Models;

namespace Youpay.API.Repository.Impl
{
    public class BankingDetailsRepository : IBankingDetailsRepository
    {
        private readonly DataContext _context;

        public BankingDetailsRepository(DataContext context)
        {
            _context = context;

        }
        public async void AddBankingDetails(BankingDetails bankingDetails)
        {
            await _context.BankingDetails.AddAsync(bankingDetails);
        }

        public  void DeleteBankingDetails(BankingDetails bankingDetails)
        {
             _context.BankingDetails.Remove(bankingDetails);
        }

        public async Task<BankingDetails> FindByAccountNumber(string accountNumber)
        {
            var bankingDetailsToFind = await _context.BankingDetails
                .FirstOrDefaultAsync(acc => acc.AccountNumber.Equals(accountNumber));

            return bankingDetailsToFind;
        }

        public async Task<BankingDetails> FindById(long id)
        {
            var bankingDetailsToFind = await _context.BankingDetails
                .FirstOrDefaultAsync(acc => acc.Id == id);

            return bankingDetailsToFind;
        }

        public async Task<bool> SaveChanges()
        {
            var isSaved =  await _context.SaveChangesAsync() > 0;
            return isSaved;
        }

        public  void UpdateBankingDetails(BankingDetails bankingDetails)
        {
             _context.Update(bankingDetails);
        }
    }
}