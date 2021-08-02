using BlazorBugTracker.Data;
using BlazorBugTracker.Data.Enums;
using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker.Utilities
{
    public static class DataManage
    {
        static int company1Id;
        static int company2Id;
        static int company3Id;

        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }
        public static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(":");

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        public static async Task ManageDataAsync(IHost host)
        {
            using var svcScope = host.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;

            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            // an instance of role manager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // an instance of usermanager
            var userManageSvc = svcProvider.GetRequiredService<UserManager<CustomUser>>();

            await dbContextSvc.Database.MigrateAsync();
            //await dbContextSvc.Database.MigrateAsync();

            // add role to the system
            await SeedRoleAsync(roleManagerSvc, userManageSvc);
            await SeedDefaultCompaniesAsync(dbContextSvc);
            await SeedDefaultUserAsync(userManageSvc, roleManagerSvc);
            await SeedDemoUsersAsync(userManageSvc, roleManagerSvc);
            await SeedDefaultTicketType(dbContextSvc);
            await SeedDefaultTicketStatusAsync(dbContextSvc);
            await SeedDefaultTicketPriorityAsync(dbContextSvc);
            await SeedDefautProjectsAsync(dbContextSvc);
            await SeedDefautTicketsAsync(dbContextSvc);
        }

        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, UserManager<CustomUser> userManager)
        {
            // call upon the roleSvc to add the new role
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Translator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Artist.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.DemoUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NormalUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.BlockUser.ToString()));
        }

        public static async Task SeedDefaultCompaniesAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Company> defaltcompanies = new List<Company>()
                {
                    new Company() { Name = "Apple", Description = "This is default Company Apple"},
                    new Company() { Name = "Tesla", Description = "This is default Company Tesla"},
                    new Company() { Name = "Netflix", Description = "This is default Company Netflix"},
                    new Company() { Name = "Temporary Company", Description = "This is Temporary Company for Deleted Company User"}
                };
                var dbCompanies = context.Company.Select(c => c.Name).ToList();
                await context.Company.AddRangeAsync(defaltcompanies.Where(c => !dbCompanies.Contains(c.Name)));
                context.SaveChanges();

                //get company Id
                company1Id = context.Company.FirstOrDefault(p => p.Name == "Apple").Id;
                company2Id = context.Company.FirstOrDefault(p => p.Name == "Tesla").Id;
                company3Id = context.Company.FirstOrDefault(p => p.Name == "Netflix").Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***************  ERROR  **************");
                Debug.WriteLine("Error Seeding Companies.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*****************************");
                throw;
            }
        }

        //Seed Default User
        public static async Task SeedDefaultUserAsync(UserManager<CustomUser> userManagerSvc, RoleManager<IdentityRole> roleManager)
        {
            //create your self as a user

            var defaultUser = new CustomUser()
            {
                Email = "arthastheking113@gmail.com",
                UserName = "arthastheking113@gmail.com",
                FirstName = "Lan",
                LastName = "Le",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 10000

            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***************  ERROR  **************");
                Debug.WriteLine("Error Seeding Default Admin");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*****************************");
                throw;
            }

            //Seed Default ProjectManager1 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay111@yahoo.com",
                Email = "mcmacay111@yahoo.com",
                FirstName = "Henry",
                LastName = "McCoy",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 4000
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default ProjectManager1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default ProjectManager2 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay112@yahoo.com",
                Email = "mcmacay112@yahoo.com",
                FirstName = "Peter",
                LastName = "Quill",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 3000
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default ProjectManager2 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer1 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay113@yahoo.com",
                Email = "mcmacay113@yahoo.com",
                FirstName = "Steve",
                LastName = "Rogers",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 4500
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Developer1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer2 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay114@yahoo.com",
                Email = "mcmacay114@yahoo.com",
                FirstName = "James",
                LastName = "Howlett",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 4400
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Developer2 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer3 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay115@yahoo.com",
                Email = "mcmacay115@yahoo.com",
                FirstName = "Natasha",
                LastName = "Romanova",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 3900
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Developer3 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer4 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay116@yahoo.com",
                Email = "mcmacay116@yahoo.com",
                FirstName = "Carol",
                LastName = "Danvers",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 3600
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Developer4 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer5 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay117@yahoo.com",
                Email = "mcmacay117@yahoo.com",
                FirstName = "Tony",
                LastName = "Stark",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 5000
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Developer5 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Submitter1 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay118@yahoo.com",
                Email = "mcmacay118@yahoo.com",
                FirstName = "Scott",
                LastName = "Summers",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 4000
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Submitter1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Default Submitter2 User
            defaultUser = new CustomUser
            {
                UserName = "mcmacay119@yahoo.com",
                Email = "mcmacay119@yahoo.com",
                FirstName = "Sue",
                LastName = "Storm",
                EmailConfirmed = true,
                CompanyId = company1Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 6000
            };
            try
            {
                var user = await userManagerSvc.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManagerSvc.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Default Submitter2 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }

        }

        //Seed Demo User
        public static async Task SeedDemoUsersAsync(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Demo Admin User
            var defaultUser = new CustomUser
            {
                UserName = "demoadmin@gmail.com",
                Email = "demoadmin@gmail.com",
                FirstName = "Demo",
                LastName = "Admin",
                EmailConfirmed = true,
                CompanyId = company2Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 10000
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Demo Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Demo ProjectManager User
            defaultUser = new CustomUser
            {
                UserName = "demopm@gmail.com",
                Email = "demopm@gmail.com",
                FirstName = "Demo",
                LastName = "ProjectManager",
                EmailConfirmed = true,
                CompanyId = company2Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 5000
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Demo ProjectManager1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Demo Developer User
            defaultUser = new CustomUser
            {
                UserName = "demodev@gmail.com",
                Email = "demodev@gmail.com",
                FirstName = "Demo",
                LastName = "Developer",
                EmailConfirmed = true,
                CompanyId = company2Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 4000
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Demo Developer1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Demo Submitter User
            defaultUser = new CustomUser
            {
                UserName = "demosub@gmail.com",
                Email = "demosub@gmail.com",
                FirstName = "Demo",
                LastName = "Submitter",
                EmailConfirmed = true,
                CompanyId = company2Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 3000
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Demo Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }


            //Seed Demo New User
            defaultUser = new CustomUser
            {
                UserName = "demonew@gmail.com",
                Email = "demonew@gmail.com",
                FirstName = "Demo",
                LastName = "NewUser",
                EmailConfirmed = true,
                CompanyId = company2Id,
                DateJoined = DateTime.Now,
                MonthlySalary = 2000
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Nhoclanro1!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Demo Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultTicketType(ApplicationDbContext context)
        {
            try
            {
                IList<TicketType> ticketTypes = new List<TicketType>()
                {
                    new TicketType(){ Name = "New Development"},
                    new TicketType(){ Name = "Run Time"},
                    new TicketType(){ Name = "UI"},
                    new TicketType(){ Name = "Maintenance"},
                    new TicketType(){ Name = "UnAssign"},
                };
                var dbTicketTypes = context.TicketType.Select(c => c.Name).ToList();
                await context.TicketType.AddRangeAsync(ticketTypes.Where(c => !dbTicketTypes.Contains(c.Name)));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***************  ERROR  **************");
                Debug.WriteLine("Error Seeding Ticket Types.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*****************************");
                throw;
            }

        }

        public static async Task SeedDefaultTicketStatusAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Status> ticketStatuses = new List<Status>() {
                    new Status() { Name = "New" },
                    new Status() { Name = "Open" },
                    new Status() { Name = "Development" },
                    new Status() { Name = "Testing" },
                    new Status() { Name = "Closed" },
                    new Status() { Name = "UnAssign" },
                };

                var dbTicketStatuses = context.Status.Select(c => c.Name).ToList();
                await context.Status.AddRangeAsync(ticketStatuses.Where(c => !dbTicketStatuses.Contains(c.Name)));
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Ticket Statuses.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultTicketPriorityAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Priority> ticketPriorities = new List<Priority>() {
                                                    new Priority() { Name = "Low" },
                                                    new Priority() { Name = "Medium" },
                                                    new Priority() { Name = "High" },
                                                    new Priority() { Name = "Urgent" },
                                                    new Priority() { Name = "UnAssign" },
                };

                var dbTicketPriorities = context.Priority.Select(c => c.Name).ToList();
                await context.Priority.AddRangeAsync(ticketPriorities.Where(c => !dbTicketPriorities.Contains(c.Name)));
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Ticket Priorities.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefautProjectsAsync(ApplicationDbContext context)
        {

            try
            {
                IList<Project> projects = new List<Project>() {
                     new Project() { CompanyId = company1Id, Name = "Build a Personal Porfolio", Description="Single page html, css & javascript page.  Serves as a landing page for candidates and contains a bio and links to all applications and challenges.", Created = DateTime.Now },
                     new Project() { CompanyId = company2Id, Name = "Build a supplemental Blog Web Application", Description="Candidate's custom built web application using .Net Core with MVC, a postgres database and hosted in a heroku container.  The app is designed for the candidate to create, update and maintain a live blog site." , Created = DateTime.Now },
                     new Project() { CompanyId = company3Id, Name = "Build an Issue Tracking Web Application", Description="A custom designed .Net Core application with postgres database.  The application is a multi tennent application designed to track issue tickets' progress.  Implemented with identity and user roles, Tickets are maintained in projects which are maintained by users in the role of projectmanager.  Each project has a team and team members.", Created = DateTime.Now  }
                };

                var dbProjects = context.Project.Select(c => c.Name).ToList();
                await context.Project.AddRangeAsync(projects.Where(c => !dbProjects.Contains(c.Name)));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Projects.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefautTicketsAsync(ApplicationDbContext context)
        {
            //Get project Ids
            int portfolioId = context.Project.FirstOrDefault(p => p.Name == "Build a Personal Porfolio").Id;
            int blogId = context.Project.FirstOrDefault(p => p.Name == "Build a supplemental Blog Web Application").Id;
            int bugtrackerId = context.Project.FirstOrDefault(p => p.Name == "Build an Issue Tracking Web Application").Id;

            //Get ticket type Ids
            int typeNewDev = context.TicketType.FirstOrDefault(p => p.Name == "New Development").Id;
            int typeRuntime = context.TicketType.FirstOrDefault(p => p.Name == "Run Time").Id;
            int typeUI = context.TicketType.FirstOrDefault(p => p.Name == "UI").Id;
            int typeMaintenance = context.TicketType.FirstOrDefault(p => p.Name == "Maintenance").Id;

            //Get ticket priority Ids
            int priorityLow = context.Priority.FirstOrDefault(p => p.Name == "Low").Id;
            int priorityMedium = context.Priority.FirstOrDefault(p => p.Name == "Medium").Id;
            int priorityHigh = context.Priority.FirstOrDefault(p => p.Name == "High").Id;
            int priorityUrgent = context.Priority.FirstOrDefault(p => p.Name == "Urgent").Id;

            //Get ticket status Ids
            int statusNew = context.Status.FirstOrDefault(p => p.Name == "New").Id;
            int statusOpen = context.Status.FirstOrDefault(p => p.Name == "Open").Id;
            int statusDev = context.Status.FirstOrDefault(p => p.Name == "Development").Id;
            int statusTest = context.Status.FirstOrDefault(p => p.Name == "Testing").Id;
            int statusClosed = context.Status.FirstOrDefault(p => p.Name == "Closed").Id;

            try
            {
                IList<Ticket> tickets = new List<Ticket>() {
                                //PORTFOLIO
                                new Ticket() {Name = "Portfolio Ticket 1", Description = "Ticket details for portfolio ticket 1", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityLow, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Portfolio Ticket 2", Description = "Ticket details for portfolio ticket 2", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityMedium, StatusId = statusOpen, TicketTypeId = typeMaintenance},
                                new Ticket() {Name = "Portfolio Ticket 3", Description = "Ticket details for portfolio ticket 3", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityHigh, StatusId = statusDev, TicketTypeId = typeUI},
                                new Ticket() {Name = "Portfolio Ticket 4", Description = "Ticket details for portfolio ticket 4", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityUrgent, StatusId = statusTest, TicketTypeId = typeRuntime},
                                new Ticket() {Name = "Portfolio Ticket 5", Description = "Ticket details for portfolio ticket 5", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityLow, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Portfolio Ticket 6", Description = "Ticket details for portfolio ticket 6", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityMedium, StatusId = statusOpen, TicketTypeId = typeMaintenance},
                                new Ticket() {Name = "Portfolio Ticket 7", Description = "Ticket details for portfolio ticket 7", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityHigh, StatusId = statusDev, TicketTypeId = typeUI},
                                new Ticket() {Name = "Portfolio Ticket 8", Description = "Ticket details for portfolio ticket 8", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = portfolioId, PriorityId = priorityUrgent, StatusId = statusTest, TicketTypeId = typeRuntime},
                                //BLOG        Name
                                new Ticket() {Name = "Blog Ticket 1", Description = "Ticket details for blog ticket 1", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityLow, StatusId = statusOpen, TicketTypeId = typeRuntime},
                                new Ticket() {Name = "Blog Ticket 2", Description = "Ticket details for blog ticket 2", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityMedium, StatusId = statusDev, TicketTypeId = typeUI},
                                new Ticket() {Name = "Blog Ticket 3", Description = "Ticket details for blog ticket 3", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeMaintenance},
                                new Ticket() {Name = "Blog Ticket 4", Description = "Ticket details for blog ticket 4", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityUrgent, StatusId = statusOpen, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Blog Ticket 5", Description = "Ticket details for blog ticket 5", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityLow, StatusId = statusDev,  TicketTypeId = typeRuntime},
                                new Ticket() {Name = "Blog Ticket 6", Description = "Ticket details for blog ticket 6", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityMedium, StatusId = statusNew,  TicketTypeId = typeUI},
                                new Ticket() {Name = "Blog Ticket 7", Description = "Ticket details for blog ticket 7", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityHigh, StatusId = statusOpen, TicketTypeId = typeMaintenance},
                                new Ticket() {Name = "Blog Ticket 8", Description = "Ticket details for blog ticket 8", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityUrgent, StatusId = statusDev,  TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Blog Ticket 9", Description = "Ticket details for blog ticket 9", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityLow, StatusId = statusNew,  TicketTypeId = typeRuntime},
                                new Ticket() {Name = "Blog Ticket 10", Description = "Ticket details for blog ticket 10", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityMedium, StatusId = statusOpen, TicketTypeId = typeUI},
                                new Ticket() {Name = "Blog Ticket 11", Description = "Ticket details for blog ticket 11", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityHigh, StatusId = statusDev,  TicketTypeId = typeMaintenance},
                                new Ticket() {Name = "Blog Ticket 12", Description = "Ticket details for blog ticket 12", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityUrgent, StatusId = statusNew,  TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Blog Ticket 13", Description = "Ticket details for blog ticket 13", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityLow, StatusId = statusOpen, TicketTypeId = typeRuntime},
                                new Ticket() {Name = "Blog Ticket 14", Description = "Ticket details for blog ticket 14", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityMedium, StatusId = statusDev,  TicketTypeId = typeUI},
                                new Ticket() {Name = "Blog Ticket 15", Description = "Ticket details for blog ticket 15", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityHigh, StatusId = statusNew,  TicketTypeId = typeMaintenance},
                                new Ticket() {Name = "Blog Ticket 16", Description = "Ticket details for blog ticket 16", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityUrgent, StatusId = statusOpen, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Blog Ticket 17", Description = "Ticket details for blog ticket 17", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = blogId, PriorityId = priorityHigh, StatusId = statusDev,  TicketTypeId = typeNewDev},
                                //BUGTRACKER  Name                                                                                                                  
                                new Ticket() {Name = "Bug Tracker Ticket 1", Description = "Ticket details for Bug Tracker ticket 1", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 2", Description = "Ticket details for Bug Tracker ticket 2", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 3", Description = "Ticket details for Bug Tracker ticket 3", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 4", Description = "Ticket details for Bug Tracker ticket 4", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 5", Description = "Ticket details for Bug Tracker ticket 5", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 6", Description = "Ticket details for Bug Tracker ticket 6", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 7", Description = "Ticket details for Bug Tracker ticket 7", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 8", Description = "Ticket details for Bug Tracker ticket 8", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now ,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 9", Description = "Ticket details for Bug Tracker ticket 9", Created = DateTimeOffset.Now,Updated = DateTimeOffset.Now,ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 10", Description = "Ticket details for Bug Tracker ticket 10", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 11", Description = "Ticket details for Bug Tracker ticket 11", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 12", Description = "Ticket details for Bug Tracker ticket 12", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 13", Description = "Ticket details for Bug Tracker ticket 13", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 14", Description = "Ticket details for Bug Tracker ticket 14", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 15", Description = "Ticket details for Bug Tracker ticket 15", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 16", Description = "Ticket details for Bug Tracker ticket 16", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 17", Description = "Ticket details for Bug Tracker ticket 17", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 18", Description = "Ticket details for Bug Tracker ticket 18", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 19", Description = "Ticket details for Bug Tracker ticket 19", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 20", Description = "Ticket details for Bug Tracker ticket 20", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 21", Description = "Ticket details for Bug Tracker ticket 21", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 22", Description = "Ticket details for Bug Tracker ticket 22", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 23", Description = "Ticket details for Bug Tracker ticket 23", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 24", Description = "Ticket details for Bug Tracker ticket 24", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 25", Description = "Ticket details for Bug Tracker ticket 25", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 26", Description = "Ticket details for Bug Tracker ticket 26", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 27", Description = "Ticket details for Bug Tracker ticket 27", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 28", Description = "Ticket details for Bug Tracker ticket 28", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 29", Description = "Ticket details for Bug Tracker ticket 29", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                                new Ticket() {Name = "Bug Tracker Ticket 30", Description = "Ticket details for Bug Tracker ticket 30", Created = DateTimeOffset.Now, Updated = DateTimeOffset.Now, ProjectId = bugtrackerId, PriorityId = priorityHigh, StatusId = statusNew, TicketTypeId = typeNewDev},
                };


                var dbTickets = context.Ticket.Select(c => c.Name).ToList();
                await context.Ticket.AddRangeAsync(tickets.Where(c => !dbTickets.Contains(c.Name)));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************  ERROR  *************");
                Debug.WriteLine("Error Seeding Tickets.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("***********************************");
                throw;
            }
        }
    }
}
