using System;
using System.Threading.Tasks;
using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Models;
using Youpay.API.Repository.Impl;
using Youpay.API.Utils;

namespace Youpay.API.Services.Impl
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenUtil _tokenUtil;
        private readonly IMapper _mapper;
        private readonly IUserUtil _userUtil;

        public AuthServices(IUserRepository userRepo, 
                            ITokenUtil tokenUtil,
                            IMapper mapper,
                            IUserUtil userUtil
                                )
        {
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
            response = new ApiResponseDto<LoginDto>(201, "Valid User Credentials", "", loginDto);
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
            var userExists = await _userRepo.UserExists(userRegistrationDto.Email);
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
               throw new Exception("Error Trying to register user please try again");
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

    }
}