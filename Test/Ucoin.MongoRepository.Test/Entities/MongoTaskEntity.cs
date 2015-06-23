using System;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.MongoRepository.Test
{
    public class MongoTaskEntity : StringKeyMongoEntity
    {
        public string Name { get; set; }

        public TaskStatusEntity TaskStatusEntity { get; set; }

        public TaskEntity TaskEntity { get; set; }
    }

    public class TaskStatusEntity
    {
        public DateTime LastRunTime { get; set; }
    }

    public class TaskEntity
    {
        public TimeSpan StartTime { get; set; }
    }
}
