using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class DeveloperOnProject
    {
        public DeveloperOnProject()
        {
            Ids = new List<string>();
            FullName = new List<string>();
        }
        public List<string> Ids { get; set; }
        public List<string> FullName { get; set; }
    }
}
