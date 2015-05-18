using System;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Framework.SqlDb.MessageLog
{
    public class MessageLogEntity : EfEntity<Guid>
    {
        public string Kind { get; set; }

        public string SourceId { get; set; }

        public string AssemblyName { get; set; }

        public string Namespace { get; set; }

        public string FullName { get; set; }

        public string TypeName { get; set; }

        public string SourceType { get; set; }

        public string CreationDate { get; set; }

        public string Payload { get; set; }
    }
}
