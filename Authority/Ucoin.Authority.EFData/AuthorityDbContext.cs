using System.Data.Entity;
using System.Reflection;
using System.Linq;
using System.Data.Entity.ModelConfiguration;
using System;
using Ucoin.Framework.SqlDb;

namespace Ucoin.Authority.EFData
{
    public sealed class AuthorityDbContext : BaseCustomDbContext
    {
        public AuthorityDbContext()
            : base("name=AuthorityDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeList = Assembly.GetExecutingAssembly().GetTypes().ToList();

            var typesToRegister = typeList
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
