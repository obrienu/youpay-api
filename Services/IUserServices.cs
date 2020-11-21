using System.Threading.Tasks;
using Youpay.API.Dtos;

namespace Youpay.API.Services
{
    public interface IUserServices
    {
        Task<ApiResponseDto<UserDto>> GetUserById(long id);
        Task<ApiResponseDto<UserDto>> GetUserByEmail( string email);
        Task<ApiResponseDto<UserDto>> UpdateUser(long id, UserRegistrationDto userRegistrationDto);
        Task<ApiResponseDto<bool>> DeleteUser(long id);

    }
}