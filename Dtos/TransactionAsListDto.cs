using System;

namespace Youpay.API.Dtos
{
    public class TransactionAsListDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Charges { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}