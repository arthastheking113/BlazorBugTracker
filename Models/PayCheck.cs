using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class PayCheck
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public long Salary { get; set; }
        public double NumberOfDayWorked { get; set; }

        [NotMapped]
        public string PayAmount { get { return $"{Salary / 4 / 6 * NumberOfDayWorked}"; } }

        public Guid Guid { get; set; }

        public bool IsApproved { get; set; }
        public bool IsSubmitted { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
