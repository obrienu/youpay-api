using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Youpay.API.Dtos;
using Youpay.API.Services;
using Youpay.API.Services.Impl;

namespace Youpay.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthServices _authServices;
        private readonly ITransactionServices _tranService;
        public AuthController(IAuthServices authServices, ITransactionServices tranService)
        {
            _tranService = tranService;
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = await _authServices.Login(userLoginDto);
            return StatusCode(response.Status, response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] UserRegistrationDto userRegistrationDto)
        {
            var response = await _authServices.Register(userRegistrationDto);
            return StatusCode(response.Status, response);
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto requestPasswordResetDto)
        {
            var response = await _authServices.RequestPasswordReset(requestPasswordResetDto.Email);
            return StatusCode(response.Status, response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto passwordResetDto)
        {
            var response = await _authServices.ResetPassword(passwordResetDto);
            return StatusCode(response.Status, response);
        }

        [HttpPost("first-time-transaction")]
        public async Task<ActionResult> FirstTimeTransactionRegistration([FromBody] FirstTimeUserTransactionRegisterationDto registerationDto)
        {
            var response = await _tranService.AddFirstTimeUserTransaction(registerationDto);
            return StatusCode(response.Status, response);
        }

    }
}