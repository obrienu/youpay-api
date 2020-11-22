namespace Youpay.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Youpay.API.Dtos;
    using global::Youpay.API.Services;
    using global::Youpay.API.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    //using Youpay.API.Models;

    namespace Youpay.API.Controllers
    {
        [Authorize]
        [Route("api/v1/user/{userId}/[controller]/")]
        [ApiController]
        public class TransactionController : ControllerBase
        {
            private readonly ITransactionServices _tranService;
            private readonly ICustomAuthorization _customAuth;
            public TransactionController(ITransactionServices tranService, ICustomAuthorization customAuth)
            {
                _customAuth = customAuth;
                _tranService = tranService;
            }

            [HttpGet("{transactionId}")]
            public async Task<ActionResult> GetTransaction(long userId, string transactionId)
            {
                if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                    return StatusCode(401);
                
                var response = await _tranService.GetTransaction(transactionId);
                return StatusCode(response.Status, response);
             
            }

            [HttpGet("")]
            public async Task<ActionResult> GetTransactions(long userId, [FromQuery] UserTransactionsParams transactionsParams)
            {
                if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                    return StatusCode(401);
                
                var response = await _tranService.GetTransactions(userId, transactionsParams);
                return StatusCode(response.Status, response);
            }

            

            [HttpPost("")]
            public async Task<ActionResult> ExixtingUserTransactionRegistration(long userId, [FromBody] UserTransactionRegistrationDto registerationDto)
            {
                if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                    return StatusCode(401);
                var response = await _tranService.AddTransactionForExistingUser(userId, registerationDto);
                return StatusCode(response.Status, response);
            }

            [HttpDelete("{transactionId}")]
            public async Task<ActionResult> DeleteTransaction(long userId, string transactionId)
            {
                if(!_customAuth.IsUserAllowedAccess(userId, HttpContext))
                    return StatusCode(401);
                
                var response = await _tranService.DeleteTransaction(transactionId, _customAuth.IsUserAdmin(HttpContext));
                return StatusCode(response.Status, response);
            }
        }
    }
}