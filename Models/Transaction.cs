using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string Reason { get; set; }
        public string UserID { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        [Required]
        public int TransactionTypeId { get; set; }
        public virtual TransactionType TransactionType { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
