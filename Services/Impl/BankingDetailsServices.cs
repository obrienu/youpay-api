using System.Threading.Tasks;
using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Helpers;
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

        /*
        * Method returns an ApiResponseDto with users banking details by the banking details id
        * It return a 404 error if the id does not match a users banking details id
        * To optimise security, method searches for banking details that are linked to the provided 
        * user Id
        */
        public async Task<ApiResponseDto<BankingDetailsDto>> GetBankingDetailsById(long userId, long bankingDetailsId)
        {
            var bankingDetails = await GetUserBankingDetailsByUserIdAndBankingDetailsId(userId, bankingDetailsId);

            if (bankingDetails == null)
            {
                return new ApiResponseDto<BankingDetailsDto>(404,
                 "Record unavailable", "Error fetching data", null);
            }
            var bankingDetailsToReturn = _mapper.Map<BankingDetailsDto>(bankingDetails);

            return new ApiResponseDto<BankingDetailsDto>(200, "Success", null, bankingDetailsToReturn);
        }
        public async Task<ApiResponseDto<bool>> DeleteBankingDetails(long userId, long bankingDetailsId)
        {
            var bankingDetails = await GetUserBankingDetailsByUserIdAndBankingDetailsId(userId, bankingDetailsId);
            if (bankingDetails == null)
            {
                return new ApiResponseDto<bool>(404,
                 "Record unavailable", "Error deleting data", false);
            }
            _bankingRepo.DeleteBankingDetails(bankingDetails);
            var isDeleted = await _bankingRepo.SaveChanges();
            if (!isDeleted)
            {
                return new ApiResponseDto<bool>(500,
                 "An Error occured while trying to delete record", "Error deleting record", false);
            }

            return new ApiResponseDto<bool>(200, "Record deleted", null, true);
        }

        public async Task<ApiResponseDto<BankingDetailsDto>> SaveBankingDetails(long userId, BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {
            var bankingDetailsInDatabase = await _bankingRepo.FindByAccountNumber(bankingDetailsRegistrationDto.AccountNumber);
            if(bankingDetailsInDatabase != null)
            {
                return new ApiResponseDto<BankingDetailsDto>(403,
                    "This record already exists", "Error adding record", null);
            }
            var bankingDetailsDto =  await SaveAccountRecord(userId, bankingDetailsRegistrationDto);
            System.Console.WriteLine("Second");
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

        public async Task<ApiResponseDto<bool>> UpdateBankingDetails(long userId, long bankingDetailsId, 
                                        BankingDetailsRegistrationDto bankingDetailsRegistrationDto)
        {
            var bankingDetails = await GetUserBankingDetailsByUserIdAndBankingDetailsId(userId, bankingDetailsId);
            
            if(bankingDetails == null)
            {
                return new ApiResponseDto<bool>(500,
                 "Record does not exist", "Record Unavailable", false);
            }

            if(bankingDetailsRegistrationDto.IsMain)
            {
                var user = await _userRepo.GetUser(userId);
                SetAllUserBankingDetailsToNotMain(user);
            }
                

            bankingDetails.AccountName = bankingDetailsRegistrationDto.AccountName;
            bankingDetails.AccountNumber = bankingDetailsRegistrationDto.AccountNumber;
            bankingDetails.AccountType = bankingDetailsRegistrationDto.AccountType.SetAccountType();
            bankingDetails.BankName = bankingDetailsRegistrationDto.BankName;
            bankingDetails.IsMain = bankingDetailsRegistrationDto.IsMain;


            _bankingRepo.UpdateBankingDetails(bankingDetails);

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
            var user = await _userRepo.GetUser(userId);
            


            if(user.BankingDetails.Count >= 3)
            {
                return null;
            }

            var bankingDetails = _mapper.Map<BankingDetails>(bankingDetailsRegistrationDto);

            if(bankingDetails.IsMain)
                SetAllUserBankingDetailsToNotMain(user);

            bankingDetails.User = user;
            
            _bankingRepo.AddBankingDetails(bankingDetails);
            
            var isSaved = await _bankingRepo.SaveChanges();
            if(!isSaved)
            {
                return null;
            }
            var bankingDetailsDto = _mapper.Map<BankingDetailsDto>(bankingDetails);
            return bankingDetailsDto;
        }

        /*
        * Method searched a user banking record for the required banking details by its id
        * It return null if non is found
        */
        private async Task<BankingDetails> GetUserBankingDetailsByUserIdAndBankingDetailsId(long userId, long bankingDetailsId)
        {
            var user = await _userRepo.GetUser(userId);

            if(user == null)
               return null;

            BankingDetails bankingDetails = null;

            foreach (var account in user.BankingDetails)
            {
                if(account.Id == bankingDetailsId)
                    bankingDetails = account;
            }
            return bankingDetails;
        }
        
        private async void  SetAllUserBankingDetailsToNotMain(User user)
        { 
            
            var count = user.BankingDetails.Count;
            var bankingDetails = user.BankingDetails;
            for (int i = 0; i < count; i++)
            {
                bankingDetails[i].IsMain = false;
                _bankingRepo.UpdateBankingDetails(bankingDetails[i]);
                await _bankingRepo.SaveChanges();
                
            }
            
        }
    }
}