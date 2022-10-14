using Duende.IdentityServer.Models;

namespace Identity.MVC;

public static class Config
{
     public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("sale.all"),
                new ApiScope("sale.read"),
                new ApiScope("sale.write"),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("sale-api", "Sale APIs")
                {
                    Scopes = { "sale.all", "sale.read", "sale.write" }
                }
            };

        public static IEnumerable<Client> Clients(IConfiguration config)
        {
            var publicClientUrl = config.GetValue("PublicClientUrl", "https://web.cs.local:5000");
            var internalClientUrl = config.GetValue("InternalClientUrl", "https://localhost:5000");
            
            return new Client[]
            {
                // BFF gateway
                new Client
                {
                    ClientId = "gw-api",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    RedirectUris = { $"{internalClientUrl}/signin-oidc", $"{publicClientUrl}/signin-oidc" },

                    BackChannelLogoutUri = $"{internalClientUrl}/logout",

                    PostLogoutRedirectUris = { $"{internalClientUrl}/signout-callback-oidc", $"{internalClientUrl}/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "sale.all" }
                },
                new Client
                {
                    ClientId = "sale-api",
                    ClientSecrets = new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = new[] {"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes = new[] { "sale.read", "sale.write" }
                }
            };
        }
}