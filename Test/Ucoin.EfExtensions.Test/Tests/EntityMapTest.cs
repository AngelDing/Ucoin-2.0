using System;
using Ucoin.Framework.EfExtensions.Mapping;
using Xunit;

namespace Ucoin.EfExtensions.Test
{
    public class EntityMapTest
    {
        [Fact]
        public void ef_extension_mapping_test()
        {
            var db = new TrackerContext();
            var resolver = new MetadataMappingProvider();

            var map = resolver.GetEntityMap(typeof(AuditData), db);

            Assert.Equal("[dbo].[Audit]", map.TableFullName);
        }
    }
}
