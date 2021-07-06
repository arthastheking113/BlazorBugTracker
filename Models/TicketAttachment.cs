using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Created { get; set; }

        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile FormFile { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
