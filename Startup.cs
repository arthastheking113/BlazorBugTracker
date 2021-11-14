using BlazorBugTracker.Areas.Identity;
using BlazorBugTracker.Data;
using BlazorBugTracker.Models;
using BlazorBugTracker.Service;
using BlazorBugTracker.Services;
using BlazorBugTracker.Utilities;
using Blazored.SessionStorage;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.CssEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(
                   DataManage.GetConnectionString(Configuration)), contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Transient);
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddIdentity<CustomUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddDefaultUI()
             .AddDefaultTokenProviders()
             .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<CustomUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext);

            services.AddTransient<IImageService, BasicImageService>();


            services.AddTransient<IEmailSender, EmailService>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddScoped<ICustomRoleService, CustomRoleService>();
            services.AddScoped<ICustomProjectService, CustomProjectService>();


            services.AddScoped<ICustomHistoryService, CustomHistoryService>();
    

            services.AddScoped<ICustomFileService, CustomFileService>();
            services.AddScoped<ICustomNotificationService, CustomNotificationService>();
            services.AddScoped<IListDeveloperAndProject, ListDeveloperandProject>();
            services.AddScoped<HubConnectionBuilder, HubConnectionBuilder>();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddCssEvents();
            services.AddJsInteropExtensions();
            services.AddBlazoredSessionStorage();
            string azureSignalrConnectionString = Configuration["Azure:ConnectionString"];
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 1024 * 15 * 15 * 15;
            }).AddAzureSignalR(options =>
            {
                options.ConnectionString = azureSignalrConnectionString;
                options.ServerStickyMode = Microsoft.Azure.SignalR.ServerStickyMode.Required;
            });
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<ChatHub>(ChatHub.HubUrl);
               
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
