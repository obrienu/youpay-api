using System.Threading.Tasks;
using Youpay.API.Dtos;

namespace Youpay.API.Services.Impl
{
    public interface IAuthServices
    {
        Task<ApiResponseDto<LoginDto>> Login(UserLoginDto userLoginDto);
        Task<ApiResponseDto<UserDto>> Register(UserRegistrationDto userRegistrationDto);
    }
}