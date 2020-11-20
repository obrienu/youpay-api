using System.Threading.Tasks;
using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Models;
using Youpay.API.Repository;
using Youpay.API.Repository.Impl;

namespace Youpay.API.Services.Impl
{
    public class BankingDetailsServices : IBankingDetailsServices
    {
        private readonly IBankingDetailsRepository _bankingRepo;
        private readonly IUserRepository _userRepo;
         private readonly IMapper _mapper;
        public BankingDetailsServices(IBankingDetailsRepository bankingRepo,
         IUserRepository userRepo, IMapper mapper )
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _bankingRepo = bankingRepo;

        }

        public async Task<ApiResponseDto<BankingDetails>> GetBankingDetailsById(long id)
        {
            var bankingDetails = await _bankingRepo.FindById(id);
            if (bankingDetails == null)
            {
                return new ApiResponseDto<BankingDetails>(404,
                 "Banking details unavailable in records", "Error fetching data", null);
            }
            return new ApiResponseDto<BankingDetails>(200, "Success", null, bankingDetails);
        }
        public async Task<ApiResponseDto<bool>> DeleteBankingDetails(long id)
        {
            var bankingDetails = await _bankingRepo.FindById(id);
            if (bankingDetails == null)
            {
                return new ApiResponseDto<bool>(404,
                 "Banking details unavailable in records", "Error deleting data", false);
            }
            _bankingRepo.DeleteBankingDetails(bankingDetails);
            var isDeleted = await _bankingRepo.SaveChanges();
            if (!isDeleted)
            {
                return new ApiResponseDto<bool>(500,
                 "An Error occured while trying to delete record", "Error deleting record", false);
            }

            return new ApiResponseDto<bool>(200, "Success", null, true);
        }

        public async Task<ApiResponseDto<BankingDetailsDto>> SaveBankingDetails(long userId, BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {
           var bankingDetailsDto =  await SaveAccountRecord(userId, bankingDetailsRegistrationDto);
            if(bankingDetailsDto == null)
            {
                return new ApiResponseDto<BankingDetailsDto>(500,
                 "An Error occured while trying to save record", "Error saving record", null);
            }
            return new ApiResponseDto<BankingDetailsDto>(200, "Record saved", null, bankingDetailsDto);
        }

        public async Task<ApiResponseDto<bool>> SetBankingDetailsAsMain(long userId, long bankingDetailsId)
        {
            var user = await _userRepo.GetUser(userId);
            var count = user.BankingDetails.Count;
            var bankingDetails = user.BankingDetails;
            for (int i = 0; i < count; i++)
            {
                if(bankingDetails[i].Id == bankingDetailsId)
                {
                    bankingDetails[i].IsMain = true;
                        _bankingRepo.UpdateBankingDetails(bankingDetails[i]);
                }
                else
                {
                    bankingDetails[i].IsMain = false;
                        _bankingRepo.UpdateBankingDetails(bankingDetails[i]);
                }
            }
            var isUpdated = await _bankingRepo.SaveChanges();
            if(!isUpdated)
            {
                return new ApiResponseDto<bool>(500,
                 "An Error occured while trying to update record", "Error updating record", false);
            }
            return new ApiResponseDto<bool>(200, "Record upated", null, true);
        }

        public async Task<ApiResponseDto<bool>> UpdateBankingDetails(long bankingDetailsId, 
                                        BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {
            var bankingDetails = await _bankingRepo.FindById(bankingDetailsId);
            
            if(bankingDetails == null)
            {
                return new ApiResponseDto<bool>(500,
                 "Record does not exist", "Record Unavailable", false);
            }

            var bankingDetailsToUpdated = _mapper.Map<BankingDetails>(bankingDetailsRegistrationDto);
            bankingDetailsToUpdated.Id = bankingDetails.Id;
            _bankingRepo.UpdateBankingDetails(bankingDetailsToUpdated);

            var isUpdated = await _bankingRepo.SaveChanges();

            if(!isUpdated)
            {
                return new ApiResponseDto<bool>(500,
                 "An Error occured while trying to update record", "Error updating record", false);
            }
            return new ApiResponseDto<bool>(200, "Record upated", null, true);
            
        }

        public async Task<BankingDetailsDto> SaveAccountRecord(long userId, BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {

            var bankingDetails = _mapper.Map<BankingDetails>(bankingDetailsRegistrationDto);
            if(bankingDetails.IsMain)
                SetAllUserBankingDetailsToNotMain(userId);
            
            _bankingRepo.AddBankingDetails(bankingDetails);
            var isSaved = await _bankingRepo.SaveChanges();
            if(!isSaved)
            {
                return null;
            }
            var bankingDetailsDto = _mapper.Map<BankingDetailsDto>(bankingDetails);
            return bankingDetailsDto;
        }
        
        private async void  SetAllUserBankingDetailsToNotMain(long userId)
        { 
            var user = await _userRepo.GetUser(userId);
            var count = user.BankingDetails.Count;
            var bankingDetails = user.BankingDetails;
            for (int i = 0; i < count; i++)
            {
                bankingDetails[i].IsMain = false;
                 _bankingRepo.UpdateBankingDetails(bankingDetails[i]);
            }
        }
    }
}