using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class InboxNotification
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public bool IsSeen { get; set; }
        public string ReceiverId { get; set; }
        public virtual CustomUser Receiver { get; set; }

        public DateTime Created { get; set; }

        public string SenderId { get; set; }
        public virtual CustomUser Sender { get; set; }

        public int InboxId { get; set; }

        public InboxNotification(string subject, string recevierId, string senderId, int inboxId)
        {
            Subject = subject;
            ReceiverId = recevierId;
            SenderId = senderId;
            InboxId = inboxId;
            Created = DateTime.Now;
        }
        public InboxNotification()
        {

        }
    }
}
