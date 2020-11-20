using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youpay.API.Models
{
    public class Transaction
    {
        [Key, Required, StringLength(50), DatabaseGenerated(DatabaseGeneratedOption.None), ]
        public string Id { get; set; }

        [Required, StringLength(100)]
        public string ProductName { get; set; }
        [Required, StringLength(100)]
        public string Category { get; set; }
        [Required]
        public int Charges { get; set; }

        [Required, StringLength(255)]
        public string Description { get; set; }
        public bool HasPaid { get; set; }
        public bool IsCanceled { get; set; }
        public bool HasShipped { get; set; }
        public bool Delivered { get; set; }
        public bool Completed { get; set; }
        public bool HasIssue { get; set; }
        public User Merchant { get; set; }
        public User Buyer { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]        
        public DateTime UpdatedAt { get; set; }
    }
}