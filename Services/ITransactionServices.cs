using System.Threading.Tasks;
using Youpay.API.Dtos;

namespace Youpay.API.Services
{
    public interface ITransactionServices
    {
        Task<ApiResponseDto<bool>> AddFirstTimeUserTransaction(FirstTimeUserTransactionRegisterationDto registerationDto);
        Task<ApiResponseDto<bool>> AddTransactionForExistingUser(long userId, UserTransactionRegistrationDto userTransactionDto);
        Task<ApiResponseDto<bool>> DeleteTransaction(long transsactionId, bool isAdmin);
        Task<ApiResponseDto<TransactionResponseDto>> GetTransaction(long transactionId);
        Task<ApiResponseDto<bool>> UpdateTransactionPaymentStatus(long userId, long transactionId, bool isAdmin);
        Task<ApiResponseDto<bool>> UpdateTransactionShipmentStatus(long userId, long transactionId, bool isAdmin);
        Task<ApiResponseDto<bool>> UpdateTransactionDeliveryStatus(long userId, long transactionId, bool isAdmin);
        Task<ApiResponseDto<PaginatedTransactionsResponseDto>> GetTransactions(long userId, UserTransactionsParams userParams);
        Task<string> GenerateTransactionCode();
    }
}