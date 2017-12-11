using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("MySecuredApi")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "adminClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //AllowedCorsOrigins = {  "http:localhost/IdentityServerApi/Identity" },
                    ClientSecrets =
                    {
                        new Secret("adminSecret".Sha256())
                    },

                    AllowedScopes = { "MySecuredApi" },
                    AccessTokenLifetime = 1800,//30 minutes
                    IdentityTokenLifetime = 1800,//30 minutes
                    Claims = new Claim[]
                    {
                        new Claim("Role", "admin")
                    }
                },
                new Client
                {
                    ClientId = "userClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //AllowedCorsOrigins = {  "http:localhost/IdentityServerApi/Identity2" },
                    ClientSecrets =
                    {
                        new Secret("userSecret".Sha256())
                    },
                    
                    AllowedScopes = { "MySecuredApi" },
                    AccessTokenLifetime = 1800,//30 minutes
                    IdentityTokenLifetime = 1800,//30 minutes
                    Claims = new Claim[]
                    {
                        new Claim("Role", "user"),
                    }
                }
            };
        }
    }
}
