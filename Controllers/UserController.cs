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
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly ICustomAuthorization _customAuth;
        public UserController(IUserServices userService, ICustomAuthorization customAuth)
        {
            _customAuth = customAuth;
            _userService = userService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            if (!_customAuth.IsUserAllowedAccess(id, HttpContext))
                return StatusCode(401, new ApiResponseDto<UserDto>(401, "Unauthorized", "Authorization error", null));

            var response = await _userService.GetUserById(id);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UserRegistrationDto userRegistrationDto)
        {
            if (!_customAuth.IsUserAllowedAccess(id, HttpContext))
                return StatusCode(401, new ApiResponseDto<UserDto>(401, "Unauthorized", "Authorization error", null));

            var response = await _userService.UpdateUser(id, userRegistrationDto);
            return StatusCode(response.Status, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            if (!_customAuth.IsUserAllowedAccess(id, HttpContext))
                return StatusCode(401, new ApiResponseDto<UserDto>(401, "Unauthorized", "Authorization error", null));

            var response = await _userService.DeleteUser(id);
            return StatusCode(response.Status, response);
        }
    }
}