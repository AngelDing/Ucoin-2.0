using System;
using Ucoin.Framework.Logging;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.Log.Entities
{
    public class BaseLog : StringKeyMongoEntity
    {
        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }

        public string Source { get; set; }

        public LogLevelType LogLevelType { get; set; }

        /// <summary>
        /// 同AppCodeType
        /// </summary>
        public string Category { get; set; }       
    }
}
