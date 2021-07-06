using BlazorBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public interface ICustomProjectService
    {
        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        public Task<IEnumerable<CustomUser>> UserOnProjectAsync(int projectId);

        public Task<IEnumerable<CustomUser>> UserNotInProjectAsync(int projectId);

        public Task AddUserToProjectAsync(string userId, int projectId);

        public Task RemoveUserFromProjectAsync(string userId, int projectId);

        public Task<IEnumerable<Project>> ListUserProjectAsync(string userId);

        public Task<IEnumerable<CustomUser>> DeveloperOnProjectAsync(int projectId);

        public Task<IEnumerable<CustomUser>> SubmitterOnProjectAsync(int projectId);


        public Task<CustomUser> ProjectManagerOnProjectAsync(int projectId);
    }
}
