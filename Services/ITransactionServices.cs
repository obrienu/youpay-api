using System.Threading.Tasks;
using Youpay.API.Dtos;

namespace Youpay.API.Services
{
    public interface ITransactionServices
    {
        Task<ApiResponseDto<bool>> AddFirstTimeUserTransaction(FirstTimeUserTransactionRegisterationDto registerationDto);
        Task<ApiResponseDto<bool>> AddTransactionForExistingUser(long userId, UserTransactionRegistrationDto userTransactionDto);
        Task<ApiResponseDto<bool>> DeleteTransaction(string transsactionId, bool isAdmin);
        Task<ApiResponseDto<TransactionResponseDto>> GetTransaction(string transactionId);
        Task<ApiResponseDto<PaginatedTransactionsResponseDto>> GetTransactions(long userId, UserTransactionsParams userParams);
    }
}