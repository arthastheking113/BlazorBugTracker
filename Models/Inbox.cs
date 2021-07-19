using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class Inbox
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public bool IsSeen { get; set; }

  
        public bool IsReceiver { get; set; }

        [Display(Name = "Sent From")]
        public string SenderId { get; set; }
        public virtual CustomUser Sender { get; set; }

        [Display(Name = "Sent To")]
        public string ReceiverId { get; set; }
        public virtual CustomUser Receiver { get; set; }


        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }


    }
}
