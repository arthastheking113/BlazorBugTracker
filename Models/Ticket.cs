using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset Created { get; set; }


        [Display(Name = "Assigned?")]
        public bool IsAssigned { get; set; }

        [Display(Name = "Updated")]
        public DateTimeOffset Updated { get; set; }

        [Display(Name = "Developer")]
        public string DeveloperId { get; set; }
        public virtual CustomUser Developer { get; set; }

        [Display(Name = "Ownner")]
        public string OwnnerId { get; set; }
        public virtual CustomUser Ownner { get; set; }

        [Display(Name = "Project")]
        [Required]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Display(Name = "Priority")]
        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; }

        [Display(Name = "Type")]
        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new HashSet<TicketHistory>();
    }
}
