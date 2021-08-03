using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class HRReport
    {
        public int Id { get; set; }

        public double HourSalary { get; set; }
        public double HoursWorked { get; set; }
        public double Salary { get; set; }
       
        public DateTime PayDate { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CustomUserSSN { get; set; }
        public string CustomUserName { get; set; }
        public string CustomUserAddress { get; set; }
        public string CustomUserPhoneNumber { get; set; }
        public string CustomUserEmail { get; set; }
  
        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }

        public string HRId { get; set; }
        public virtual CustomUser HR { get; set; }
    }
}
