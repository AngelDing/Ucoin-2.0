using System;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.Log.Entities
{
    public class BasePerfLog : StringKeyMongoEntity
    {
        public string Unit { get; set; }

        public long Ticks { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
