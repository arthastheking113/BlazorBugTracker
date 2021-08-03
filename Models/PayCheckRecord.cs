using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class PayCheckRecord
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public float NumberHourOfWorked { get; set; }
        public double Salary { get; set; }
        public double UserSalary { get; set; }
        
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFinished { get; set; }

        public List<int> PayRollId { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
