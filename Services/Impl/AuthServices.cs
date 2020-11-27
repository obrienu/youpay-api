using System;
using System.Threading.Tasks;
using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Models;
using Youpay.API.Repository;
using Youpay.API.Utils;

namespace Youpay.API.Services.Impl
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenUtil _tokenUtil;
        private readonly IMapper _mapper;
        private readonly IUserUtil _userUtil;
        private readonly IMailingServices _mailingServices;

        public AuthServices(
                            IMailingServices mailingServices,
                            IUserRepository userRepo, 
                            ITokenUtil tokenUtil,
                            IMapper mapper,
                            IUserUtil userUtil
                            
                                )
        {
            _mailingServices = mailingServices;
            _mapper = mapper;
            _userUtil = userUtil;
            _tokenUtil = tokenUtil;
            _userRepo = userRepo;
        }

        public  async Task<ApiResponseDto<LoginDto>> Login(UserLoginDto userLoginDto)
        {
            ApiResponseDto<LoginDto> response = null;
            var userToLogin = await _userRepo.FindUserByEmail(userLoginDto.Email.ToLower());
            if (userToLogin == null)
            {
                response = new ApiResponseDto<LoginDto>(401, "Invalid User Credentials", "Authentication error", null);
                return response;
            }
                

            if(!VerifyPasswordHash(userLoginDto.Password, userToLogin.PasswordHash, userToLogin.PasswordSalt))
            {
                response = new ApiResponseDto<LoginDto>(401, "Invalid User Credentials", "Authentication error", null);
                return response;
            }

            if(!userToLogin.IsVerified){
                 VerifyUser(userToLogin);
            }
            
            var tokenClaims = new TokenClaimsDto()
            {
                Id = userToLogin.Id,
                Email = userToLogin.Email,
                Role = "User"
            };

            var token = _tokenUtil.GenerateToken(tokenClaims);
            var loginDto = new LoginDto()
            {
                Token = token,
                UserId = userToLogin.Id,
                Email = userToLogin.Email,
                Role = "User"
            };
            response = new ApiResponseDto<LoginDto>(201, "Valid User Credentials", null, loginDto);
            return response;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
           using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
           {
                var comparedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if(comparedHash[i] != passwordHash[i]) return false;
                }
           }
           return true;
        }
        
        public async Task<ApiResponseDto<UserDto>> Register(UserRegistrationDto userRegistrationDto)
        {
            var userExists = await _userRepo.UserExists(userRegistrationDto.Email, userRegistrationDto.PhoneNumber);
            if( userExists)
            {
                 return new ApiResponseDto<UserDto>()
                        {
                            Status = 400,
                            Message = "User already Exists",
                            Data = null,
                            Error = "Authentication Error"
                        };
            }

            var password = _userUtil.GenerateRandomPassword(8);
            System.Console.WriteLine(password);
            var user = _mapper.Map<User>(userRegistrationDto);
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
             _userRepo.SaveUser(user);
            var saved = await _userRepo.SaveChanges();
            if(!saved){
               return new ApiResponseDto<UserDto>(500,
                 "An Error occured while trying to register user", "Error registering user", null);
            }

            var isPasswordSent = await _mailingServices.SendPassword(user.Firstname, user.Email, password);
            if(!isPasswordSent)
            {
                var userToDelete = await _userRepo.FindUserByEmail(user.Email);
                _userRepo.DeleteUser(userToDelete);
                await _userRepo.SaveChanges();
                return new ApiResponseDto<UserDto>(500,
                 "An Error occured while trying to register user", "Error registering user", null);
            }

            var userToReturn = _mapper.Map<UserDto>(user);
            return new ApiResponseDto<UserDto>()
            {
                Status = 201,
                Message = "User registered successfully",
                Data = userToReturn,
                Error = null
            };
        }
        

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async void VerifyUser(User user)
        {
            user.IsVerified = true;
            _userRepo.UpdateUser(user);
           await _userRepo.SaveChanges();
        }

        public async Task<ApiResponseDto<bool>> RequestPasswordReset(string email)
        {
            var user = await _userRepo.FindUserByEmail(email);
            if(user == null)
            {
                return new ApiResponseDto<bool>(404, "User record not found", "Error reseting password", false);
            }
            var token = _userUtil.GenerateRandomId(8);
            var tokenExpiringDate = DateTime.Now.AddMinutes(10);
            System.Console.WriteLine(token);
            user.PasswordResetToken = token;
            user.ResetExpiresAt = tokenExpiringDate;
            _userRepo.UpdateUser(user);
            var isUpdated = await _userRepo.SaveChanges();
            if(!isUpdated)
            {
                return new ApiResponseDto<bool>(500, 
                        "An error occured while trying to request password reset", 
                            "Error reseting password", false);
            }

            var isPasswordSent = await _mailingServices.SendPasswordResetToken(user.Firstname, token, user.Email);
            if(!isPasswordSent)
            {
                var userToDelete = await _userRepo.FindUserByEmail(user.Email);
                _userRepo.DeleteUser(userToDelete);
                await _userRepo.SaveChanges();
                return new ApiResponseDto<bool>(500,
                 "An Error occured while trying to register user", "Error registering user", false);
            }

            return new ApiResponseDto<bool>(200, "Password reset link has been sent to users email", null, true);

        }

        public async Task<ApiResponseDto<bool>> ResetPassword(PasswordResetDto passwordResetDto)
        {
            var user = await _userRepo.FindUserByResetToken(passwordResetDto.Token);
            if(user == null)
            {
                return new ApiResponseDto<bool>(404, "Invalid password reset token", "Error reseting password", false);
            }

            int compareDateTime = DateTime.Compare(user.ResetExpiresAt, DateTime.Now);

            if(compareDateTime < 0)
            {
                return new ApiResponseDto<bool>(400, "Expired reset token, please request for a new reset token", "Error reseting password", false);
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(passwordResetDto.Password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            _userRepo.UpdateUser(user);
            var isUpdated = await _userRepo.SaveChanges();

            if(!isUpdated)
            {
                return new ApiResponseDto<bool>(500, "We encountered an error while trying to reset your password, please try again", 
                "Error reseting password", false);
            }

            return new ApiResponseDto<bool>(200, "Your password has been updated, login with your new password", 
                null, true);
        }

    }
}