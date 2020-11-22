using System.Threading.Tasks;
using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Repository;

namespace Youpay.API.Services.Impl
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserServices(IUserRepository userRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo = userRepo;
        }
        public async Task<ApiResponseDto<bool>> DeleteUser(long id)
        {
            var userToDelete = await _userRepo.GetUser(id);

            if(userToDelete == null)
            {
                return new ApiResponseDto<bool>(400, "User record not found", "Error deleting record", false);
            }

             _userRepo.DeleteUser(userToDelete);
            
            if(!await _userRepo.SaveChanges())
            {
                return new ApiResponseDto<bool>(500, "An error occured while trying to delete user",
                                    "Error deleting user", false);
            }

            return new ApiResponseDto<bool>(200, "User deleted", null, true);

        }

        public async Task<ApiResponseDto<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userRepo.FindUserByEmail(email);
            if(user == null)
            {
                return new ApiResponseDto<UserDto>(400, "User record not found", "Error fetching record", null);
            }

            var userToSend =  _mapper.Map<UserDto>(user);

            return new ApiResponseDto<UserDto>(200, "Sucess", null, userToSend);
        }

        public async Task<ApiResponseDto<UserDto>> GetUserById(long id)
        {
             var user = await _userRepo.GetUser(id);
            if(user == null)
            {
                return new ApiResponseDto<UserDto>(404, "User record not found", "Error fetching record", null);
            }

            var userToSend =  _mapper.Map<UserDto>(user);

            return new ApiResponseDto<UserDto>(200, "Sucess", null, userToSend);
        }

       

        public async Task<ApiResponseDto<UserDto>> UpdateUser(long id, UserRegistrationDto userRegistrationDto)
        {
            var user = await _userRepo.GetUser(id);

            if(user == null)
            {
                return new ApiResponseDto<UserDto>(404, "User record not found", "Error fetching record", null);
            }
            user.Firstname = userRegistrationDto.Firstname;
            user.LastName = userRegistrationDto.LastName;
            user.Email = userRegistrationDto.Email;
            user.PhoneNumber = userRegistrationDto.PhoneNumber;

            _userRepo.UpdateUser(user);
            
            if(!await _userRepo.SaveChanges())
            {
                return new ApiResponseDto<UserDto>(500, "An error occured while trying to update user",
                                    "Error updating user", null);
            }

            var updatedUser = _mapper.Map<UserDto>(user);

            return new ApiResponseDto<UserDto>(200, "User details updated", null, updatedUser);
        }
    }
}