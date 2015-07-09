using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Models;

namespace Ucoin.Identity.ServerHost.IdSvr
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new Scope[]
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.OfflineAccess,
                StandardScopes.Roles,
                new Scope
                {
                    Name = "read",
                    DisplayName = "Read data",
                    Type = ScopeType.Resource,
                    Emphasize = false,
                },
                new Scope
                {
                    Name = "write",
                    DisplayName = "Write data",
                    Type = ScopeType.Resource,
                    Emphasize = true,
                },
             };
        }
    }
}