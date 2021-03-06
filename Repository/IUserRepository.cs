using System.Collections.Generic;
using System.Threading.Tasks;
using Youpay.API.Models;

namespace Youpay.API.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUser(long id);
        Task<List<User>> GetUsers();
        Task<User> FindUserByEmail(string email);
        Task<User> FindUserByResetToken(string token);
        void SaveUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveChanges();
        Task<bool> UserExists(string email, string phoneNumber);


    }
}