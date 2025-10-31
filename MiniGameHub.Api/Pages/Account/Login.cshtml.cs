// MiniGameHub.Api/Pages/Account/Login.cshtml.cs

using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duende.IdentityServer.Services; 
using Duende.IdentityServer.Events;    
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;


namespace MiniGameHub.Api.Pages.Account.Login
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction; 
        private readonly IEventService _events;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interaction, 
            IEventService events)                        
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _events = events;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();
        
        public class InputModel
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public string ReturnUrl { get; set; } = "/";
        }

        public void OnGet(string? returnUrl)
        {
            Input.ReturnUrl = returnUrl ?? "/";
        }
        
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var result =  await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(Input.Email);
                    
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id, user.Email));

                    /*var isuser = new IdentityServerUser(user.SubjectId)
                    {
                        DisplayName = user.Username
                    };
                    await HttpContext.SignInAsync(
                        IdentityServerConstants.DefaultCookieAuthenticationScheme,
                        isuser.CreatePrincipal());*/
                    
                    if (_interaction.IsValidReturnUrl(Input.ReturnUrl) || Url.IsLocalUrl(Input.ReturnUrl))
                    {
                        return Redirect(Input.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
            }

            return Page();
        }
    }
}