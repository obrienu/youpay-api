using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Youpay.API.Helpers;

namespace Youpay.API.Models
{
    public class User
    {
        
        public User()
        {
            BankingDetails = new List<BankingDetails>();
        }
        
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(6)]
        public Gender Sex { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [StringLength(6)]
        public string PasswordResetToken { get; set; }
        public DateTime ResetExpiresAt { get; set; }
        public virtual ICollection<BankingDetails> BankingDetails { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]        
        public DateTime UpdatedAt { get; set; }

    }
}