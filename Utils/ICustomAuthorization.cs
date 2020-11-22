using Microsoft.AspNetCore.Http;

namespace Youpay.API.Utils
{
    public interface ICustomAuthorization
    {
        bool IsUserAllowedAccess(long userId, HttpContext _context);
        bool IsUserAdmin(HttpContext _context);
    }
}