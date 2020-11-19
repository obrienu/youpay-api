
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Youpay.API.Dtos;
using Youpay.API.Services.Impl;

namespace Youpay.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthServices _authServices;
        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login( UserLoginDto userLoginDto)
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


    }
}