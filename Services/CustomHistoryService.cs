using BlazorBugTracker.Data;
using BlazorBugTracker.Models;
using BlazorBugTracker.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public class CustomHistoryService : ICustomHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public CustomHistoryService(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            if (oldTicket.Name != newTicket.Name)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Name",
                    OldValue = oldTicket.Name,
                    NewValue = newTicket.Name,
                    Created = DateTime.Now,
                    CustomUserId = userId
                };
                await _context.TicketHistory.AddAsync(history);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Description",
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Created = DateTime.Now,
                    CustomUserId = userId
                };
                await _context.TicketHistory.AddAsync(history);
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Ticket Type",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    Created = DateTime.Now,
                    CustomUserId = userId
                };
                await _context.TicketHistory.AddAsync(history);
            }

            if (oldTicket.PriorityId != newTicket.PriorityId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Priority",
                    OldValue = oldTicket.Priority.Name,
                    NewValue = newTicket.Priority.Name,
                    Created = DateTime.Now,
                    CustomUserId = userId
                };
                await _context.TicketHistory.AddAsync(history);
            }


            if (oldTicket.StatusId != newTicket.StatusId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Status",
                    OldValue = oldTicket.Status.Name,
                    NewValue = newTicket.Status.Name,
                    Created = DateTime.Now,
                    CustomUserId = userId
                };
                await _context.TicketHistory.AddAsync(history);
            }
            if (oldTicket.DeveloperId != null && newTicket.DeveloperId != null)
            {
                if (oldTicket.DeveloperId != newTicket.DeveloperId)
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = oldTicket.Developer.FullName,
                        NewValue = newTicket.Developer.FullName,
                        Created = DateTime.Now,
                        CustomUserId = userId
                    };
                    await _context.TicketHistory.AddAsync(history);
                }
            }
            var currentStatus = _context.Status.FirstOrDefault(t => t.Name == "Closed").Id; // new 

            if (newTicket.StatusId == currentStatus) 
            {
                Notification notification = new Notification
                {
                    Name = $"Your Ticket: {newTicket.Name} is Done",
                    TicketId = newTicket.Id,
                    Description = $"Yay!!! Your Ticket {newTicket.Name} on project {_context.Project.FirstOrDefault(p => p.Id == newTicket.ProjectId).Name} is Approved",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperId
                };
                await _context.Notification.AddAsync(notification);
                await _context.SaveChangesAsync();

                string devEmail = newTicket.Developer.Email;
                string subject = "Your Ticket is Approved";
                string message = $"Your Ticket on project: {_context.Project.FirstOrDefault(p => p.Id == newTicket.ProjectId).Name} has been Aaproved!!!";

                await _emailSender.SendEmailAsync(devEmail, subject, message);

                await _context.SaveChangesAsync();
            }
     
            //send email
            if (newTicket.IsAssigned == true && newTicket.StatusId != currentStatus)
            {
                Notification notification = new Notification
                {
                    Name = $"New Ticket Assign.",
                    TicketId = newTicket.Id,
                    Description = $"You have a new ticket {newTicket.Name} about {newTicket.Description} on project: {_context.Project.FirstOrDefault(p => p.Id == newTicket.ProjectId).Name}",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperId
                };
                await _context.Notification.AddAsync(notification);
                await _context.SaveChangesAsync();


                string devEmail = newTicket.Developer.Email;
                string subject = "New Ticket Assignment";
                string message = $"You have a new ticket for project: {_context.Project.FirstOrDefault(p => p.Id == newTicket.ProjectId).Name}";

                await _emailSender.SendEmailAsync(devEmail, subject, message);

                await _context.SaveChangesAsync();
            }
   
        }
    }
}
