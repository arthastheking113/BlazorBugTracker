using BlazorBugTracker.Data;
using BlazorBugTracker.Data.Enums;
using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public class CustomRoleService : ICustomRoleService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<CustomUser> _userManager;

        public CustomRoleService(ApplicationDbContext dbContext, UserManager<CustomUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<bool> AddUserToRoleAsync(CustomUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRoleAsync(CustomUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> ListUserRoleAsync(CustomUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<string> ReturnUserRole(CustomUser user)
        {
            var role = await _userManager.GetRolesAsync(user);
            var returnRole = "";
            if (role.Count != 0)
            {
                returnRole = role.ToList().First().ToString();
            }
            return returnRole;
        }
        public string ReturnUserRole2(CustomUser user)
        {
            var roleId = _dbContext.UserRoles.FirstOrDefault(s => s.UserId == user.Id);
            var returnRole = "Not In Role";
            if (roleId is null)
            {
                returnRole = _dbContext.Roles.FirstOrDefault(s => s.Id == roleId.ToString()).Name;
            }
            return returnRole;
        }
        public string ReturnUserRole3(CustomUser user)
        {
            var roleId = _dbContext.UserRoles.Where(s => s.UserId == user.Id).ToList();
            var returnRole = "Not In Role";
            if (roleId is null)
            {
                foreach (var item in roleId)
                {
                    returnRole = "";
                    var role = _dbContext.Roles.FirstOrDefault(s => s.Id == item.ToString()).Name;
                    returnRole += $"{role},";
                }
                returnRole.Remove(returnRole.Length - 1);

            }
            return returnRole;
        }
        public IEnumerable<IdentityRole> NonDemoRoles()
        {
            var roles = _dbContext.Roles;
            var output = new List<IdentityRole>();
            foreach (var role in roles)
            {
                if (role.Name != Roles.DemoUser.ToString())
                {
                    output.Add(role);
                }
            }
            return output;
        }

        public async Task<bool> RemoveUserFromRoleAsync(CustomUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<CustomUser>> UsersInRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        public async Task<IEnumerable<CustomUser>> UsersNotInRoleAsync(string role)
        {
            var inRole = await UsersInRoleAsync(role);
            var users = await _dbContext.Users.ToListAsync();
            return users.Except(inRole);
        }
    }
}
