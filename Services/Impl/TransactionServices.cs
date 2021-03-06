using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Youpay.API.Dtos;
using Youpay.API.Models;
using Youpay.API.Repository;
using Youpay.API.Utils;

namespace Youpay.API.Services.Impl
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IBankingDetailsRepository _accountRepo;
        private readonly ITransactionsRepository _transRepo;
        private readonly IAuthServices _authServices;
        private readonly IBankingDetailsServices _bankingService;
        private readonly IUserUtil _userUtil;
        private readonly IMapper _mapper;

        private readonly IMailingServices _mailingServices;

        public TransactionServices(IMailingServices mailingServices, IMapper mapper, IAuthServices authServices, IBankingDetailsServices bankingService, IUserUtil userUtil,
            IUserRepository userRepo, IBankingDetailsRepository accountRepo, ITransactionsRepository transRepo)
        {
            _mailingServices = mailingServices;
            _mapper = mapper;
            _userUtil = userUtil;
            _bankingService = bankingService;
            _authServices = authServices;
            _transRepo = transRepo;
            _accountRepo = accountRepo;
            _userRepo = userRepo;
        }
        public async Task<ApiResponseDto<bool>> AddFirstTimeUserTransaction(FirstTimeUserTransactionRegisterationDto registerationDto)
        {
            var merchantToRegister = registerationDto.Merchant;
            var merchantBankingDetails = registerationDto.MerchantBankingDetails;
            var buyerToRegiser = registerationDto.Buyer;
            var buyerBankingDetails = registerationDto.BuyerBankingDetails;

            await _authServices.Register(merchantToRegister);
            await _authServices.Register(buyerToRegiser);
            

            var merchant = await _userRepo.FindUserByEmail(merchantToRegister.Email);
            var buyer = await _userRepo.FindUserByEmail(buyerToRegiser.Email);

            if (merchant == null || buyer == null)
            {
                return new ApiResponseDto<bool>(500, "An error occured while creating transaction, please try again",
                 "Error creating transaction", false);
            }


            await _bankingService.SaveBankingDetails((long)merchant.Id, merchantBankingDetails);
            await _bankingService.SaveBankingDetails((long)buyer.Id, buyerBankingDetails);

            var transaction = new Transaction()
            {
                Code = await GenerateTransactionCode(),
                ProductName = registerationDto.ProductName,
                Category = registerationDto.Category,
                Charges = registerationDto.Charges,
                Description = registerationDto.Description,
                Merchant = merchant,
                Buyer = buyer
            };

            _transRepo.AddTransaction(transaction);
            var isUpdated = await _transRepo.SaveChanges();

            if (!isUpdated)
            {
                return new ApiResponseDto<bool>(500, "An error occured while creating transaction, login in to your account and try again",
                "Error creating transaction", false);
            }
            await _mailingServices.SendTransactionMail(merchant, buyer);
            await _mailingServices.SendTransactionMail(buyer, merchant);

            return new ApiResponseDto<bool>(200, "Transaction successfully created, check your email for transaction details", null, true);

        }

        public async Task<ApiResponseDto<bool>> AddTransactionForExistingUser(long userId, UserTransactionRegistrationDto userTransactionDto)
        {
            var user = await _userRepo.GetUser(userId);

            await _authServices.Register(userTransactionDto.SecondParty);

            var secondParty = await _userRepo.FindUserByEmail(userTransactionDto.SecondParty.Email);

            if (user == null || secondParty == null)
            {
                return new ApiResponseDto<bool>(500, "An error occured while creating transaction, login in to your account and try again",
                "Error creating transaction", false);
            }

            await _bankingService.SaveAccountRecord((long)secondParty.Id, userTransactionDto.SecondPartyBankingDetails);

            var transaction = new Transaction()
            {
                Code = await GenerateTransactionCode(),
                ProductName = userTransactionDto.ProductName,
                Category = userTransactionDto.Category,
                Charges = userTransactionDto.Charges,
                Description = userTransactionDto.Description,
            };

            if (userTransactionDto.SecondPartyAs.ToLower().Equals("merchant"))
            {
                transaction.Merchant = secondParty;
                transaction.Buyer = user;
            }
            else
            {
                transaction.Buyer = secondParty;
                transaction.Merchant = user;
            }

            _transRepo.AddTransaction(transaction);
            var isUpdated = await _transRepo.SaveChanges();

            if (!isUpdated)
            {
                return new ApiResponseDto<bool>(500, "An error occured while creating transaction, login in to your account and try again",
                "Error creating transaction", false);
            }

            await _mailingServices.SendTransactionMail(user, secondParty);
            await _mailingServices.SendTransactionMail(secondParty, user);

            return new ApiResponseDto<bool>(200, "Transaction successfully created, check your email for transaction details", null, true);

        }

        public async Task<ApiResponseDto<bool>> UpdateTransactionPaymentStatus(long userId, long transactionId, bool isAdmin)
        {
            var transaction = await _transRepo.FindTransactionById(transactionId);
            if (transaction == null )
            {
                return new ApiResponseDto<bool>(404, "Record not found", "Error updating record", false);
            }

            if(transaction.Buyer.Id != userId || !isAdmin)
            {
                return new ApiResponseDto<bool>(403, "User not authorized to carry out this operation", "Error updating record", false);
            }

            transaction.HasPaid = true;
            _transRepo.UpdateTransaction(transaction);
            var isUpdated = await _transRepo.SaveChanges();
            if (!isUpdated)
            {
                return new ApiResponseDto<bool>(500, "An error occured while updating transaction",
                "Error updating transaction", false);
            }

            await _mailingServices.sendNotificationOfPayment(transaction);

            return new ApiResponseDto<bool>(200, "Transaction successfully updated", null, true);

        }
        public async Task<ApiResponseDto<bool>> UpdateTransactionShipmentStatus(long userId, long transactionId, bool isAdmin)
        {
            var transaction = await _transRepo.FindTransactionById(transactionId);

            if (transaction == null )
            {
                return new ApiResponseDto<bool>(404, "Record not found", "Error updating record", false);
            }

            if(transaction.Buyer.Id != userId || !isAdmin)
            {
                return new ApiResponseDto<bool>(403, "User not authorized to carry out this operation", "Error updating record", false);
            }

            transaction.HasShipped = true;
            _transRepo.UpdateTransaction(transaction);
            var isUpdated = await _transRepo.SaveChanges();

            if (!isUpdated)
            {
                return new ApiResponseDto<bool>(500, "An error occured while updating transaction",
                "Error updating transaction", false);
            }

            await _mailingServices.sendNotificationOfPayment(transaction);

            return new ApiResponseDto<bool>(200, "Transaction successfully updated", null, true);

        }
        public async Task<ApiResponseDto<bool>> UpdateTransactionDeliveryStatus(long userId,
                                                                                long transactionId,
                                                                                bool isAdmin)
        {
            var transaction = await _transRepo.FindTransactionById(transactionId);

            if (transaction == null )
            {
                return new ApiResponseDto<bool>(404, "Record not found", "Error updating record", false);
            }

            if(transaction.Buyer.Id != userId || !isAdmin)
            {
                return new ApiResponseDto<bool>(403, "User not authorized to carry out this operation", "Error updating record", false);
            }

            transaction.Delivered = true;
            _transRepo.UpdateTransaction(transaction);
            var isUpdated = await _transRepo.SaveChanges();
            if (!isUpdated)
            {
                return new ApiResponseDto<bool>(500, "An error occured while updating transaction",
                "Error updating transaction", false);
            }

            await _mailingServices.sendNotificationOfDelivery(transaction);

            return new ApiResponseDto<bool>(200, "Transaction successfully updated", null, true);

        }

        public async Task<ApiResponseDto<bool>> DeleteTransaction(long transactionId,
                                                                  bool isAdmin)
        {

            var transaction = await _transRepo.FindTransactionById(transactionId);

            if (transaction == null)
            {
                return new ApiResponseDto<bool>(404, "Transaction record not found", "Eror deleting record", false);
            }

            if (transaction.HasPaid && !transaction.Delivered && !isAdmin)
            {
                return new ApiResponseDto<bool>(403, "Transaction already paid for but not delivered cannot be deleted, please contact the admin for any issue related to this transaction", "Eror deleting record", false);
            }

            _transRepo.DeleteTransaction(transaction);
            var isDeleted = await _transRepo.SaveChanges();
            if (!isDeleted)
            {
                return new ApiResponseDto<bool>(500, "We encoured a problem while trying to delete this record", "Error deleting record", false);
            }

            return new ApiResponseDto<bool>(200, "Record deleted successfully", null, true);
        }

        public async Task<ApiResponseDto<TransactionResponseDto>> GetTransaction(long transactionId)
        {
            var transaction = await _transRepo.FindTransactionById(transactionId);

            if (transaction == null)
            {
                return new ApiResponseDto<TransactionResponseDto>(404, "Record not found", "Error fetching record", null);
            }

            var transactionToReturn = _mapper.Map<TransactionResponseDto>(transaction);

            return new ApiResponseDto<TransactionResponseDto>(200, "Sucess", null, transactionToReturn);

        }

        public async Task<ApiResponseDto<PaginatedTransactionsResponseDto>> GetTransactions(long userId, UserTransactionsParams userParams)
        {
            var transactions = await _transRepo.FindUsersTransaction(userParams, userId);
            if (transactions.TotalCount == 0)
            {
                return new ApiResponseDto<PaginatedTransactionsResponseDto>(404, "No records found for this user", "Error fetching records", null);
            }

            var transactionsToReturn = _mapper.Map<IEnumerable<TransactionAsListDto>>(transactions);

            var paginatedResponse = new PaginatedTransactionsResponseDto()
            {
                Transactions = transactionsToReturn,
                PageSize = transactions.PageSize,
                TotalPages = transactions.TotalPages,
                Count = transactions.TotalCount,
                CurrentPage = transactions.CurrentPage
            };

            return new ApiResponseDto<PaginatedTransactionsResponseDto>(200, "Success", null, paginatedResponse);
        }

        public async Task<string> GenerateTransactionCode()
        {
            var lastGeneratedCode = await _transRepo.GetLastGeneratedCode();
            if (string.IsNullOrWhiteSpace(lastGeneratedCode))
            {
                return "YOU0000001";
            }

            var stringDigit = lastGeneratedCode.Substring(3);
            var nextDigit = (Int64.Parse(stringDigit) + 1).ToString().PadLeft(7, '0');
            return "YOU" + nextDigit;
        }

    }
}