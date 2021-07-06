using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
