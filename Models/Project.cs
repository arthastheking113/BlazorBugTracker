using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    //what does this model do?
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "Upload Image")]
        public Byte[] ImageData { get; set; }
        public string ContentType { get; set; }

        public virtual ICollection<CustomUser> CustomUsers { get; set; } = new HashSet<CustomUser>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<ProjectAttachment> Attachments { get; set; } = new HashSet<ProjectAttachment>();

    }
}
