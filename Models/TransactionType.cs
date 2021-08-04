using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
