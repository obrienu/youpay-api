using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Youpay.API.Utils.Impl
{
    public class CustomAuthorization :  ICustomAuthorization 
    {
        public bool IsUserAllowedAccess(long userId,  HttpContext _context)
        {
            var userIdClaim = _context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoleClaim = _context.User.FindFirstValue(ClaimTypes.Role);

            if(userIdClaim == null && userRoleClaim == null)
                return false;         
  
            if(!userIdClaim.Equals(userId.ToString())  && !userRoleClaim.Equals("Admin"))
                return false;
            
            return true;
            
        }
    }
}