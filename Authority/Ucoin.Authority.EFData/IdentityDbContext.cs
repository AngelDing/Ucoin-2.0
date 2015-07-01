using System.Data.Entity;
using System.Reflection;
using System.Linq;
using System.Data.Entity.ModelConfiguration;
using System;
using Ucoin.Framework.SqlDb;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Ucoin.Authority.EFData
{
    public sealed class IdentityDbContext : BaseCustomDbContext
    {
        public IdentityDbContext()
            : base("name=IdentityDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除复数表名的契约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

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
