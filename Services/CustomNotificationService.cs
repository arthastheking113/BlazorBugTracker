using BlazorBugTracker.Data;
using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public class CustomNotificationService : ICustomNotificationService
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CustomNotificationService(UserManager<CustomUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IEnumerable<Notification>> GetUnReadNotificationAsync(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            var notifications = _context.Notification.Where(n => n.RecipientId == user.Id).Include(n => n.Sender).Include(n => n.Recipient);
            var unreadNotifications = await notifications.Where(n => !n.IsViewed).ToListAsync();
            return unreadNotifications;
        }
    }
}
