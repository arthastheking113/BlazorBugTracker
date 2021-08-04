using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project> Project { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketAttachment> Attachment { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Invite> Invite { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<ProjectAttachment> ProjectAttachment { get; set; }
        public DbSet<InboxNotification> InboxNotification { get; set; }
        public DbSet<WelcomeNotification> WelcomeNotification { get; set; }
        public DbSet<PayRoll> PayRoll { get; set; }
        public DbSet<PayCheckRecord> PayCheckRecord { get; set; }
        public DbSet<HRReport> HRReport { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }

    }
}
