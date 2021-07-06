using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class Invite
    {
        public int Id { get; set; }
       
        public string Email { get; set; }
        public Guid CompanyToken { get; set; }
        public DateTimeOffset InviteDate { get; set; }
        
       
        public bool IsValid { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "Invitor")]
        public string InvitorId { get; set; }
        public virtual CustomUser Invitor { get; set; }


        [Display(Name = "Invitee")]
        public string InviteeId { get; set; }
        public virtual CustomUser Invitee { get; set; }

    }
}
