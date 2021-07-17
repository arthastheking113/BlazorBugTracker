﻿using BlazorBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public interface ICustomProjectService
    {
        public bool IsUserOnProject(string userId, int projectId);

        public IEnumerable<CustomUser> UserOnProject(int projectId);

        public Task<IEnumerable<CustomUser>> UserNotInProjectAsync(int projectId);

        public void AddUserToProject(List<string> userId, int projectId);

        public void RemoveUserFromProject(string userId, int projectId);

        public Task<IEnumerable<Project>> ListUserProjectAsync(string userId);

        public IEnumerable<CustomUser> DeveloperOnProject(int projectId);

        public IEnumerable<CustomUser> SubmitterOnProject(int projectId);


        public CustomUser ProjectManagerOnProject(int projectId);
    }
}
