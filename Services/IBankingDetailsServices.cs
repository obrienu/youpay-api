using System.Threading.Tasks;
using Youpay.API.Dtos;
using Youpay.API.Models;

namespace Youpay.API.Services
{
    public interface IBankingDetailsServices
    {
         Task<ApiResponseDto<BankingDetailsDto>> SaveBankingDetails(long userId, BankingDetailsRegistrationDto bankingDetailsRegistrationDto); 
         Task<ApiResponseDto<bool>> DeleteBankingDetails(long id);
         Task<ApiResponseDto<bool>> SetBankingDetailsAsMain(long userId, long bankingDetailsId);
         Task<ApiResponseDto<bool>> UpdateBankingDetails(long bankingDetailsId, BankingDetailsRegistrationDto bankingDetails);
         Task<ApiResponseDto<BankingDetails>> GetBankingDetailsById(long id);

    }
}