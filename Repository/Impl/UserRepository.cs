using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Youpay.API.Data;
using Youpay.API.Models;

namespace Youpay.API.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;

        }
        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _context.Users.Include(user => user.BankingDetails)
                .FirstOrDefaultAsync(user => user.Email == email);
            return user;
        }

        public async Task<User> GetUser(long id)
        {
            var userToFind = await _context.Users.Include(user => user.BankingDetails)
                .FirstOrDefaultAsync(user => user.Id == id);
                return userToFind;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> SaveChanges()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public async void SaveUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUser(User user)
        {
             _context.Update(user);
        }

        public async Task<bool> UserExists(string email)
        {
            if(await _context.Users.AnyAsync(user => user.Email.Equals(email)))
            {
                return true;
            }
            return false;
        }
    }
}