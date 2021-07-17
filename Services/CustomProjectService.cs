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
        public void AddUserToProject(List<string> userIdList, int projectId)
        {
            try
            {
                List<CustomUser> UserInProjectList = new();
                Project project = _context.Project.FirstOrDefault(p => p.Id == projectId);
                var AllUser = _context.Users.ToList();
                UserInProjectList = project.CustomUsers.ToList();
                foreach (var userId in userIdList)
                {
                    if (!IsUserOnProject(userId, projectId))
                    {

                        CustomUser user = AllUser.FirstOrDefault(u => u.Id == userId);
                        var IsUserInRole = _roleService.ReturnUserRole3(user);
                        if (IsUserInRole.Contains(Roles.ProjectManager.ToString()))
                        {
                            var oldManager = ProjectManagerOnProject(projectId);
                            if (oldManager != null)
                            {
                                RemoveUserFromProject(oldManager.Id, projectId);
                            }
                        }
                        UserInProjectList.Add(user);
                    }

                }

                try
                {
                    project.CustomUsers = UserInProjectList;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** ERROR *** - Error Adding user to project - message:{ex.Message}");
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine($"*** ERROR *** - Error Adding user to project - message:{ex.Message}");
            }

        }

        public IEnumerable<CustomUser> DeveloperOnProject(int projectId)
        {
            var roles = _context.Roles.FirstOrDefault(r => r.Name == Roles.Developer.ToString());
            var user = _context.UserRoles.Where(u => u.RoleId == roles.Id).Select(u => u.UserId).ToList();
            List<CustomUser> developers = new List<CustomUser>();
            foreach (var item in user)
            {
                var eachUser = _context.Users.FirstOrDefault(u => u.Id == item);
                developers.Add(eachUser);
            }
            var onProject = UserOnProject(projectId);
            var developerOnProject = developers.Intersect(onProject).ToList();
            return developerOnProject;
        }

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = _context.Project.Include(p => p.CustomUsers).First(c => c.Id == projectId);
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
                if (IsUserOnProject(userId, project.Id))
                {
                    output.Add(project);
                }
            }
            return output;
        }

        public CustomUser ProjectManagerOnProject(int projectId)
        {
            var roles = _context.Roles.FirstOrDefault(r => r.Name == Roles.ProjectManager.ToString());
            var user = _context.UserRoles.Where(u => u.RoleId == roles.Id).Select(u => u.UserId).ToList();
            List<CustomUser> userList = new List<CustomUser>();
            foreach (var item in user)
            {
                var eachUser = _context.Users.FirstOrDefault(u => u.Id == item);
                userList.Add(eachUser);
            }
            CustomUser manager = new CustomUser();
            var onProject = UserOnProject(projectId);
            manager = userList.Intersect(onProject.ToList()).FirstOrDefault();
            return manager;
        }

        public void  RemoveUserFromProject(string userId, int projectId)
        {
            try
            {
                if (IsUserOnProject(userId, projectId))
                {
                    CustomUser user = _context.Users.FirstOrDefault(u => u.Id == userId);
                    Project project = _context.Project.FirstOrDefault(u => u.Id == projectId);
                    try
                    {
                        project.CustomUsers.Remove(user);
                        _context.SaveChanges();
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

        public IEnumerable<CustomUser> SubmitterOnProject(int projectId)
        {
            var roles = _context.Roles.FirstOrDefault(r => r.Name == Roles.Submitter.ToString());
            var user = _context.UserRoles.Where(u => u.RoleId == roles.Id).Select(u => u.UserId).ToList();
            List<CustomUser> submitters = new List<CustomUser>();
            foreach (var item in user)
            {
                var eachUser = _context.Users.FirstOrDefault(u => u.Id == item);
                submitters.Add(eachUser);
            }
            var onProject = UserOnProject(projectId);
            var submitterOnProject = submitters.Intersect(onProject).ToList();
            return submitterOnProject;
        }

        public async Task<IEnumerable<CustomUser>> UserNotInProjectAsync(int projectId)
        {
            var output = new List<CustomUser>();
            var userss = await _context.Users.ToListAsync();
            foreach (var user in userss)
            {
                if (!IsUserOnProject(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;
        }

        public  IEnumerable<CustomUser> UserOnProject(int projectId)
        {
            var output = new List<CustomUser>();
            var userss = _context.Users.ToList();
            foreach (var user in userss)
            {
                if(IsUserOnProject(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;
        }
    }
}
