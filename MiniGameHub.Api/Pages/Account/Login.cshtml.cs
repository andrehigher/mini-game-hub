// MiniGameHub.Api/Pages/Account/Login.cshtml.cs

using Duende.IdentityServer.Test;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Duende.IdentityServer.Services; 
using Duende.IdentityServer.Events;    
using System.Security.Claims;
using Duende.IdentityServer; // Necessário para a autenticação baseada em Claims


namespace MiniGameHub.Api.Pages.Account.Login
{
    // A classe principal: LoginModel
    public class LoginModel : PageModel
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction; 
        private readonly IEventService _events;

        // O construtor com as injeções
        public LoginModel(
            TestUserStore users, 
            IIdentityServerInteractionService interaction, 
            IEventService events)                        
        {
            _users = users;
            _interaction = interaction;
            _events = events;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        // A classe interna para o modelo do formulário
        public class InputModel
        {
            // ... (campos: Username, Password, ReturnUrl)
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string ReturnUrl { get; set; } = "/";
            // ...
        }

        public void OnGet(string? returnUrl)
        {
            Input.ReturnUrl = returnUrl ?? "/";
        }

        // O método chamado ao submeter o formulário
        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine($"Username: {Input.Username}, Password: {Input.Password}");
            // O conteúdo corrigido do OnPost que eu te passei anteriormente...
            if (ModelState.IsValid)
            {
                if (_users.ValidateCredentials(Input.Username, Input.Password))
                {
                    var user = _users.FindByUsername(Input.Username);
                    
                    // Disparar evento de sucesso (boa prática)
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username));

                    // Autenticar e criar a sessão (cookie) do IdentityServer
                    var isuser = new IdentityServerUser(user.SubjectId)
                    {
                        DisplayName = user.Username
                    };
                    await HttpContext.SignInAsync(
                        IdentityServerConstants.DefaultCookieAuthenticationScheme,
                        isuser.CreatePrincipal());
                    
                    // O REDIRECIONAMENTO CORRETO para continuar o fluxo OIDC
                    if (_interaction.IsValidReturnUrl(Input.ReturnUrl) || Url.IsLocalUrl(Input.ReturnUrl))
                    {
                        return Redirect(Input.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            }

            return Page();
        }
    }
}