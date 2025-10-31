using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniGameHub.Api.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;

        public LogoutModel(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        [BindProperty]
        public string? LogoutId { get; set; }

        public async Task<IActionResult> OnGet(string logoutId)
        {
            await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);
            return Redirect(logoutContext?.PostLogoutRedirectUri ?? "~/");
        }

        public async Task<IActionResult> OnPostAsync(string logoutId)
        {
            // Remove o cookie de autenticação
            await HttpContext.SignOutAsync(
                IdentityServerConstants.DefaultCookieAuthenticationScheme);

            // Obtém info sobre o logout (pode incluir o post_logout_redirect_uri)
            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

            // Redireciona de volta para o client (SPA)
            if (!string.IsNullOrEmpty(logoutContext?.PostLogoutRedirectUri))
            {
                return Redirect(logoutContext.PostLogoutRedirectUri);
            }

            // fallback
            return Redirect("~/");
        }
    }
}