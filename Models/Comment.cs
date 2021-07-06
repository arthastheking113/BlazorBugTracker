using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsModerated { get; set; }
        public DateTime Moderated { get; set; }
        [Display(Name = "Moderated Reason")]
        public string ModeratedReason { get; set; }
        [Display(Name = "Moderated Content")]
        public string ModeratedContent { get; set; }

        [Display(Name = "User")]
        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }

        [Display(Name = "Ticket")]
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
