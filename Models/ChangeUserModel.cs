using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class ChangeUserModel
    {
        public int ProjectId { get; set; }
        public string ProjectManagerId { get; set; }
        public List<string> Developerid { get; set; }
        public List<string> SubmitterId { get; set; }
    }
}
