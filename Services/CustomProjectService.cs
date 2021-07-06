using BlazorBugTracker.Data;
using BlazorBugTracker.Data.Enums;
using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public class CustomProjectService : ICustomProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomRoleService _roleService;

        public CustomProjectService(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor, ICustomRoleService roleService)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _roleService = roleService;
        }
        public async Task AddUserToProjectAsync(string userId, int projectId)
        {
            try
            {
                if (!await IsUserOnProjectAsync(userId, projectId))
                {
                    CustomUser user = await _userManager.FindByIdAsync(userId);
                    if ( await _roleService.IsUserInRoleAsync(user, Roles.ProjectManager.ToString()))
                    {
                        var oldManager = await ProjectManagerOnProjectAsync(projectId);
                        if (oldManager != null)
                        {
                            await RemoveUserFromProjectAsync(oldManager.Id, projectId);
                        }
                    }
                    Project project = await _context.Project.FindAsync(projectId);
                    try
                    {
                        project.CustomUsers.Add(user);
                        await _context.SaveChangesAsync();
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"*** ERROR *** - Error Adding user to project - message:{ex.Message}");
            }

        }

        public async Task<IEnumerable<CustomUser>> DeveloperOnProjectAsync(int projectId)
        {
            var developers = await _userManager.GetUsersInRoleAsync(Roles.Developer.ToString());
            var onProject = await UserOnProjectAsync(projectId);
            var developerOnProject = developers.Intersect(onProject).ToList();
            return developerOnProject;
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            var project = await _context.Project.Include(p => p.CustomUsers).FirstAsync(c => c.Id == projectId);
            var user = project.CustomUsers.Any(u => u.Id == userId);
            return user;
        }

        public async Task<IEnumerable<Project>> ListUserProjectAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (await _roleService.IsUserInRoleAsync(user, Roles.Admin.ToString()) )
            {
                return await _context.Project.ToListAsync();
            }
            var output = new List<Project>();
            foreach (var project in await _context.Project.ToListAsync())
            {
                if (await IsUserOnProjectAsync(userId, project.Id))
                {
                    output.Add(project);
                }
            }
            return output;
        }

        public async Task<CustomUser> ProjectManagerOnProjectAsync(int projectId)
        {
            var projectManager = await _userManager.GetUsersInRoleAsync(Roles.ProjectManager.ToString());
            var onProject = await UserOnProjectAsync(projectId);
            var manager = projectManager.Intersect(onProject.ToList()).FirstOrDefault();
            return manager;
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                if (await IsUserOnProjectAsync(userId, projectId))
                {
                    CustomUser user = await _userManager.FindByIdAsync(userId);
                    Project project = await _context.Project.FindAsync(projectId);
                    try
                    {
                        project.CustomUsers.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** ERROR *** - Error Remove user to project - message:{ex.Message}");
            }

        }

        public async Task<IEnumerable<CustomUser>> SubmitterOnProjectAsync(int projectId)
        {
            var submitters = await _userManager.GetUsersInRoleAsync(Roles.Submitter.ToString());
            var onProject = await UserOnProjectAsync(projectId);
            var submitterOnProject = submitters.Intersect(onProject).ToList();
            return submitterOnProject;
        }

        public async Task<IEnumerable<CustomUser>> UserNotInProjectAsync(int projectId)
        {
            var output = new List<CustomUser>();
            var userss = await _context.Users.ToListAsync();
            foreach (var user in userss)
            {
                if (!await IsUserOnProjectAsync(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;
        }

        public async Task<IEnumerable<CustomUser>> UserOnProjectAsync(int projectId)
        {
            var output = new List<CustomUser>();
            var userss = await _context.Users.ToListAsync();
            foreach (var user in userss)
            {
                if(await IsUserOnProjectAsync(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;
        }
    }
}
