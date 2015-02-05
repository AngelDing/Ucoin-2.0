
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Ucoin.Framework.MongoDb.Managers
{
    public class MongoManager : BaseMongoDB<BsonDocument>, IMongoManager
    {
        public MongoManager(string sName)
            : this(sName, ConstHelper.AdminDBName)
        { 
        }

        public MongoManager(string sName, string dbName)
            : this(sName, dbName, null)
        { 
        }

        public MongoManager(string sName, string dbName, string cName)
            : base(string.Format(ConstHelper.ConnString, sName, dbName), cName)
        {
        }

        public CommandResult GetServerInfo()
        {
            var cr = DB.RunCommand(new CommandDocument { { "serverStatus", 1 } });
            return cr;
        }

        public CommandResult GetDatabaseInfo()
        {
            var cr = DB.RunCommand(new CommandDocument { { "dbstats", 1 } });
            return cr;
        }

        public CommandResult GetDatabaseList()
        {
            var cr = DB.RunCommand(new CommandDocument { { "listDatabases", 1 } });
            return cr;
        }

        public CommandResult GetCollectionInfo()
        {
            var cr = DB.RunCommand(new CommandDocument { { "collstats", this.Collection.Name } });
            return cr;
        }

        public MongoCollection<BsonDocument> GetCollection(string collName)
        {
            return DB.GetCollection(collName);
        }

        public MongoCursor<BsonDocument> GetCollectionIndexs(string collName, string nameSpace)
        {
            var coll = GetCollection(collName);
            var result = coll.Find(new QueryDocument { { "ns", nameSpace } });
            return result;
        }

        public IEnumerable<string> GetCollectionNames()
        {
            return DB.GetCollectionNames();
        }

        public GetProfilingLevelResult GetProfilingLevel()
        {
            return DB.GetProfilingLevel();
        }

        public MongoCursor<SystemProfileInfo> GetProfilingInfo(IMongoQuery query, int limit)
        {
            return DB.GetProfilingInfo(query).SetLimit(limit);
        }

        public CommandResult SetProfilingLevel(ProfilingLevel level, TimeSpan timeSpan)
        {
            return DB.SetProfilingLevel(level, timeSpan);
        }

        public CommandResult SetProfilingLevel(ProfilingLevel level)
        {
            return DB.SetProfilingLevel(level);
        }


        public CommandResult GetReplicationInfo()
        {
            return DB.RunCommand(new CommandDocument { { "ismaster", 1 } });
        }
    }
}
