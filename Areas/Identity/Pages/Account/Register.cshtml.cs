using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BlazorBugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using BlazorBugTracker.Data;
using BlazorBugTracker.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlazorBugTracker.Data.Enums;

namespace BlazorBugTracker.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly ICustomRoleService _roleService;

        public RegisterModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, ApplicationDbContext context
            , ICustomRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Company")]
            public int CompanyId { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            await _signInManager.SignOutAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new CustomUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, CompanyId = _context.Company.FirstOrDefault(c => c.Name == "New User Company").Id };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _roleService.AddUserToRoleAsync(user, Roles.NewUser.ToString());
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    var admin = await _userManager.FindByEmailAsync("arthastheking113@gmail.com");
                    WelcomeNotification welcomenotification = new WelcomeNotification
                    {
                        Name = "Welcome To The Bug Tracker",
                        Description = $"Hello {user.FullName} You have created a new account in my bug tracker service. Your role is New User and you are working at {_context.Company.FirstOrDefault(c => c.Id == user.CompanyId).Name}. Please wait our admin or project manager assign you a higher role in the system. You can contact our staff by the inbox system. My recommendation is to Login as a Demo User, you can see everything my Bug Tracker can do..",
                        Created = DateTime.Now,
                        SenderId = (admin).Id,
                        RecipientId = user.Id
                    };
                    await _context.WelcomeNotification.AddAsync(welcomenotification);
                    await _context.SaveChangesAsync();

                    WelcomeNotification welcomenotification2 = new WelcomeNotification
                    {
                        Name = "New Account have been created",
                        Description = $"A new user have been created on {DateTime.Now} with Full Name is: {user.FullName} and Email is: {user.Email}",
                        Created = DateTime.Now,
                        SenderId = (admin).Id,
                        RecipientId = (admin).Id
                    };
                    await _context.WelcomeNotification.AddAsync(welcomenotification2);
                    await _context.SaveChangesAsync();


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
