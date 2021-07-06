using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CustomUser> CustomUsers { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
