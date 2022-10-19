using Duende.IdentityServer;
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

        public static IEnumerable<Client> Clients()
        {
            return new Client[]
            {
                // BFF gateway
                new Client
                {
                    ClientId = "web",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser =true,
                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "sale-api",
                    ClientSecrets = new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = new[] {"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes = new[] { "sale.read", "sale.write" }
                },
                new Client
                {
                    ClientId = "postman",

                    ClientSecrets = { new Secret("postman_secret".Sha256())},

                    ClientName = "Postman password credential flow",

                    RedirectUris = { "https://localhost:5001/signin-oidc" },

                    BackChannelLogoutUri = "https://localhost:5001/bff/backchannel",

                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    AllowOfflineAccess = true,

                    AllowedScopes = { "openid", "profile", "api" }
                }
            };
        }
}