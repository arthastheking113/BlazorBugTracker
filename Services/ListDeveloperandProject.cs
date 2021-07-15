using BlazorBugTracker.Data;
using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public class ListDeveloperandProject : IListDeveloperAndProject
    {
        private readonly ICustomProjectService _projectService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public ListDeveloperandProject(ICustomProjectService projectService,
            ApplicationDbContext context,
            UserManager<CustomUser> userManager)
        {
            _projectService = projectService;
            _context = context;
            _userManager = userManager;
        }

        public  (IEnumerable<CustomUser>, List<Project>) ReturnDeveloperAndProject(CustomUser User)
        {
            IEnumerable<CustomUser> allDeveloper = new List<CustomUser>();
            var allProject = _context.Project.ToList();
            List<Project> listProject = new List<Project>();
            foreach (var item in allProject)
            {
                var developer = _projectService.DeveloperOnProject(item.Id);
                allDeveloper = allDeveloper.Union(developer);


                if (_projectService.IsUserOnProject(User.Id, item.Id))
                {
                    listProject.Add(item);
                }
            }

            return (allDeveloper, listProject);
        }

        public  IEnumerable<CustomUser> ReturnDeveloperOnly()
        {

            IEnumerable<CustomUser> allDeveloper = new List<CustomUser>();
            var allProject = _context.Project.ToList();
            foreach (var item in allProject)
            {
                var developer = _projectService.DeveloperOnProject(item.Id);
                allDeveloper = allDeveloper.Union(developer);
            }

            return allDeveloper;
        }
    }
}
