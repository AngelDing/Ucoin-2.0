using System.Data.Entity.Migrations;

namespace Ucoin.Authority.EFData
{
    internal sealed class Configuration : DbMigrationsConfiguration<IdentityDbContext>
    {
        public Configuration()
        {
            //获取或设置 指示迁移数据库时是否可使用自动迁移的值。
            AutomaticMigrationsEnabled = true;
            //获取或设置 指示是否可接受自动迁移期间的数据丢失的值。如果设置为false，则将在数据丢失可能作为自动迁移一部分出现时引发异常。
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Ucoin.Authority.EFData.IdentityDbContext";
        }

        protected override void Seed(IdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
