using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class PayCheckRecord
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public float NumberDayOfWorked { get; set; }
        public long Salary { get; set; }

        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }

        public List<int> PayRollId { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
