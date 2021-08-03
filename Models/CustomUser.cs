using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class CustomUser : IdentityUser
    {

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} character long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

    
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} character long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Change Avatar")]
        public Byte[] ImageData { get; set; }
        public string ContentType { get; set; }

        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        [NotMapped]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        public string NewConfirmPassword { get; set; }

        public string UserId { get; set; }

        public DateTime DateJoined { get; set; }

        public string SSN { get; set; }
        [NotMapped]
        public string Role { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public double MonthlySalary { get; set; }
        [NotMapped]
        public double HourSalary { get { return (MonthlySalary / 4 / 5 / 8); } }
         [NotMapped]
        public double DaySalary { get { return (MonthlySalary / 4 / 5); } }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PayRoll> PayRolls { get; set; }

        public virtual ICollection<PayCheckRecord> PayCheckRecords { get; set; }
    }
}
