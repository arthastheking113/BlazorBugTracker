using BlazorBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Services
{
    public interface IListDeveloperAndProject
    {
        public IEnumerable<CustomUser> ReturnDeveloperOnly();

        public (IEnumerable<CustomUser>, List<Project>) ReturnDeveloperAndProject(CustomUser User);
    }
}
