﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Models
{
    public class PayRoll
    {
        public int Id { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Note { get; set; }
        [NotMapped]
        public string Summary { get { return $"{Name} - {End.Subtract(Start).TotalHours}"; } }

        public bool IsSubmitted { get; set; }

        public bool IsApproved { get; set; }

        public string CustomUserId { get; set; }
        public virtual CustomUser CustomUser { get; set; }
    }
}
