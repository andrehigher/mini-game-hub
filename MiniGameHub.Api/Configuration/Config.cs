using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace MiniGameHub.Api.Configuration;

public static class Config
{
    // 1. Recursos de Identidade (ID Token Claims)
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(), // OIDC padrão
            new IdentityResources.Profile(), // Nome, etc.
        };

    // 2. Escopos de API (Access Token)
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("minigames_api", "Mini Games Hub API") // Escopo para a sua API
        };

    // 3. Clientes (seu BFF/Frontend Host)
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "minigames_hub_client",
                ClientName = "MiniGames Hub Client",
                AllowedGrantTypes = GrantTypes.Code, // OIDC Code Flow
                RequirePkce = true, 
                RequireClientSecret = false, // Não precisa de segredo com PKCE no BFF

                // URL de redirecionamento após o login
                RedirectUris = { "https://localhost:5173/signin-oidc" }, // Use a porta do seu Vite/React

                // URL de redirecionamento após o logout
                PostLogoutRedirectUris = { "https://localhost:5173" },
                AllowedCorsOrigins = { "https://localhost:5173" },

                AllowedScopes = { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "minigames_api" // Escopo que a API precisará
                },
                AllowAccessTokensViaBrowser = true // Necessário para SPA/BFF
            }
        };
}