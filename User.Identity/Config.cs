using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("geteway_api","api geteway")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                new Client{
                    ClientId="user_client",
                    ClientSecrets=new List<Secret>
                    {
                        new Secret("xb880929".Sha256())
                    },
                    RefreshTokenExpiration=TokenExpiration.Sliding,
                    AllowOfflineAccess=true,
                    RequireClientSecret=false,
                    AllowedGrantTypes=new List<string>{"sms_auth_code"},
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowedScopes=new List<string>{
                        "geteway_api",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
                new IdentityResources.Profile(),
                new IdentityResources.OpenId()
            };
        }
    }
}
