using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Models;

namespace Youpay.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Sex, opt => 
                    opt.MapFrom( src => src.Sex.SetGenderFromEnum()));

            CreateMap<BankingDetails, BankingDetailsDto>()
                .ForMember(dest => dest.AccountType, opt => 
                        opt.MapFrom(src => src.AccountType.SetAccountTypeFromEnum()));

            CreateMap<BankingDetailsRegistrationDto, BankingDetails>()
                .ForMember(dest => dest.AccountType, opt => 
                    opt.MapFrom( src => src.AccountType.SetAccountType()));

            CreateMap<UserRegistrationDto, User>()
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex.SetGender()));
        }
    }
}