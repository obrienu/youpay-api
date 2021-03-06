using System.Threading.Tasks;
using Youpay.API.Dtos;

namespace Youpay.API.Services
{
    public interface IAuthServices
    {
        Task<ApiResponseDto<LoginDto>> Login(UserLoginDto userLoginDto);
        Task<ApiResponseDto<UserDto>> Register(UserRegistrationDto userRegistrationDto);
        Task<ApiResponseDto<bool>> RequestPasswordReset(string email);
        Task<ApiResponseDto<bool>> ResetPassword(PasswordResetDto passwordResetDto);
    }
}