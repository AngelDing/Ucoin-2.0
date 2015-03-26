using System.Data.Entity;
using Ucoin.Framework.EfExtensions.Audit;

namespace Ucoin.Framework.EfExtensions
{
    public static class AuditExtensions
    {
        public static AuditLogger BeginAudit(this DbContext dbContext)
        {
            return new AuditLogger(dbContext);
        }
    }
}