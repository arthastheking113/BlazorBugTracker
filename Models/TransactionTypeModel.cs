using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class TransactionTypeModel
    {
        public DateTime Time { get; set; }
        public decimal Amount { get; set; }
    }
}
