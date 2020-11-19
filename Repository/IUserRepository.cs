using System.Collections.Generic;
using System.Threading.Tasks;
using Youpay.API.Models;

namespace Youpay.API.Repository.Impl
{
    public interface IUserRepository
    {
        Task<User> GetUser(long id);
        Task<List<User>> GetUsers();
        Task<User> FindUserByEmail(string email);
        void SaveUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveChanges();
        Task<bool> UserExists(string email);

    }
}