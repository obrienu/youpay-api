using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Youpay.API.Helpers;

namespace Youpay.API.Models
{
    public class BankingDetails
    {
        [Key]
        public long? Id { get; set; }
       
        [Required, StringLength(100)]
        public string BankName { get; set; }

        [Required, StringLength(200)]
        public string AccountName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required, StringLength(20)]
        public AccountType AccountType { get; set; }
        public bool IsMain { get; set; }
        [Required]
        public User User { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]        
        public DateTime UpdatedAt { get; set; }
    }
}