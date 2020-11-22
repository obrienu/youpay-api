using System.Collections.Generic;

namespace Youpay.API.Dtos
{
    public class PaginatedTransactionsResponseDto
    {
        public IEnumerable<TransactionAsListDto> Transactions {get; set;}
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
}