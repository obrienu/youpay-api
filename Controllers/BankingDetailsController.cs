using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Youpay.API.Dtos;
using Youpay.API.Services;
using Youpay.API.Utils;

namespace Youpay.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/user/{userId}/[controller]")]
    public class BankingDetailsController : ControllerBase
    {
        private readonly IBankingDetailsServices _bankingDetailsServices;
        private readonly ICustomAuthorization _customAuth;
        public BankingDetailsController(IBankingDetailsServices bankingDetailsServices, ICustomAuthorization customAuth)
        {
            _customAuth = customAuth;
            _bankingDetailsServices = bankingDetailsServices;
        }


        [HttpPost]
        public async Task<IActionResult> SaveBankingDetails(long userId,
         [FromBody] BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {
            if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                return StatusCode(401);
            var response = await _bankingDetailsServices.SaveBankingDetails(userId, bankingDetailsRegistrationDto);
            return StatusCode(response.Status, response);
        }

        [HttpGet("{bankingDetailsId}")]
        public async Task<IActionResult> GetBankingDetails(long userId, long bankingDetailsId)
        {
            if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                return StatusCode(401);
            var response = await _bankingDetailsServices.GetBankingDetailsById(userId, bankingDetailsId);
            return StatusCode(response.Status, response);
        }

         [HttpPut("{bankingDetailsId}/main")]
        public async Task<IActionResult> MakeBankingDetailsMainAccount(long userId, long bankingDetailsId)
        {
            if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                return StatusCode(401);
            var response = await _bankingDetailsServices.SetBankingDetailsAsMain(userId, bankingDetailsId);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{bankingDetailsId}")]
        public async Task<IActionResult> UpdateBankingDetails(long userId, long bankingDetailsId, [FromBody] BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {
            if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                return StatusCode(401);
            var response = await _bankingDetailsServices.UpdateBankingDetails(userId, bankingDetailsId, bankingDetailsRegistrationDto);
            return StatusCode(response.Status, response);
        }

       

        [HttpDelete("{bankingDetailsId}")]
        public async Task<IActionResult> DeleteBankingDetails(long userId, long bankingDetailsId)
        {
            if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                return StatusCode(401);
            var response = await _bankingDetailsServices.DeleteBankingDetails(userId, bankingDetailsId);
            return StatusCode(response.Status, response);
        }
    }
}