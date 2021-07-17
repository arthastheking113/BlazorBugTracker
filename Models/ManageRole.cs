using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class ManageRole
    {
        public string RoleSelected { get; set; }
        public List<string> UserSelected { get; set; }
    }
}
