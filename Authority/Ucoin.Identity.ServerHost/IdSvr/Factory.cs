using System.Linq;
using System.Collections.Generic;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.EntityFramework;
using Thinktecture.IdentityServer.Core.Models;
using Ucoin.Authority.Services;

namespace Ucoin.Identity.ServerHost.IdSvr
{
    class Factory
    {
        public static IdentityServerServiceFactory Configure(string connString)
        {
            var efConfig = new EntityFrameworkServiceOptions {
                ConnectionString = connString,
                //Schema = "dbo"
            };

            var cleanup = new TokenCleanup(efConfig, 10);
            cleanup.Start();

            ConfigureClients(Clients.Get(), efConfig);
            ConfigureScopes(Scopes.Get(), efConfig);

            var factory = new IdentityServerServiceFactory();

            factory.RegisterConfigurationServices(efConfig);
            factory.RegisterOperationalServices(efConfig);

            factory.CorsPolicyService = new ClientConfigurationCorsPolicyRegistration(efConfig);

            factory.UserService = new Registration<IUserService, UserService>();

            return factory;
        }

        public static void ConfigureClients(IEnumerable<Client> clients, EntityFrameworkServiceOptions options)
        {
            using (var db = new ClientConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Clients.Any())
                {
                    foreach (var c in clients)
                    {
                        var e = c.ToEntity();
                        db.Clients.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }

        public static void ConfigureScopes(IEnumerable<Scope> scopes, EntityFrameworkServiceOptions options)
        {
            using (var db = new ScopeConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Scopes.Any())
                {
                    foreach (var s in scopes)
                    {
                        var e = s.ToEntity();
                        db.Scopes.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
