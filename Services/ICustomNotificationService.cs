using BlazorBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public interface ICustomNotificationService
    {
        public Task<IEnumerable<Notification>> GetUnReadNotificationAsync(ClaimsPrincipal currentUser);
    }
}
