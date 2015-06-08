﻿using StackExchange.Redis;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;
using System.Linq;
using System;
using Ucoin.Framework.Redis;

namespace Ucoin.Framework.RedisSession
{
    public sealed class RedisConnectionWrapper
    {
        private static Dictionary<string, ConnectionMultiplexer> RedisConnections =
            new Dictionary<string, ConnectionMultiplexer>();
        private static Dictionary<string, long> RedisStats =
            new Dictionary<string, long>();

        private static Timer connMessagesSentTimer;

        private static object RedisCreateLock = new object();

        static RedisConnectionWrapper()
        {
            connMessagesSentTimer = new System.Timers.Timer(30000);
            connMessagesSentTimer.Elapsed += RedisConnectionWrapper.GetConnectionsMessagesSent;
            connMessagesSentTimer.Start();
        }

        /// <summary>
        /// Gets or sets the parameters to use when connecting to a redis server
        /// </summary>
        private ConfigurationOptions connData;

        /// <summary>
        /// A string identifier for the connection, which will be used as the connection's key in the
        ///     this.RedisConnections dictionary.
        /// </summary>
        public string ConnectionID { get; set; }

        /// <summary>
        /// The index of Database to store session.
        /// </summary>
        public int DatabaseIndex { get; set; }


        public RedisConnectionWrapper()
        {
            var redisFactory = new StackExchangeRedisFactory();

            this.connData = redisFactory.ConstructConnectionOptions();
            this.DatabaseIndex = 0; //redisFactory.;
            this.ConnectionID = "Default Redis Connection";
        }

        /// <summary>
        /// Method that returns a StackExchange.Redis.IDatabase object with ip and port number matching
        ///     what was passed into the constructor for this instance of RedisConnectionWrapper
        /// </summary>
        /// <returns>An open and callable RedisConnection object, shared with other threads in this
        /// application domain that also called for a connection to the specified ip and port</returns>
        public IDatabase GetConnection()
        {
            if (!RedisConnectionWrapper.RedisConnections.ContainsKey(this.ConnectionID))
            {
                lock (RedisConnectionWrapper.RedisCreateLock)
                {
                    if (!RedisConnectionWrapper.RedisConnections.ContainsKey(this.ConnectionID))
                    {
                        RedisConnectionWrapper.RedisConnections.Add(
                            this.ConnectionID,
                            ConnectionMultiplexer.Connect(
                                this.connData));
                    }
                }
            }

            return RedisConnectionWrapper.RedisConnections[this.ConnectionID].GetDatabase(this.DatabaseIndex);
        }

        /// <summary>
        /// Gets the number of redis commands sent and received, and sets the count to 0 so the next time
        ///     we will not see double counts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void GetConnectionsMessagesSent(object sender, ElapsedEventArgs e)
        {
            bool logCount = RedisConnectionConfig.LogConnectionActionsCountDel != null;

            if (logCount)
            {
                foreach (string connName in RedisConnectionWrapper.RedisConnections.Keys.ToList())
                {
                    try
                    {
                        ConnectionMultiplexer conn;
                        if (RedisConnectionWrapper.RedisConnections.TryGetValue(connName, out conn))
                        {
                            long priorPeriodCount = 0;
                            if (RedisConnectionWrapper.RedisStats.ContainsKey(connName))
                            {
                                priorPeriodCount = RedisConnectionWrapper.RedisStats[connName];
                            }

                            ServerCounters counts = conn.GetCounters();
                            long curCount = counts.Interactive.OperationCount;

                            // log the sent commands
                            RedisConnectionConfig.LogConnectionActionsCountDel(
                                connName,
                                curCount - priorPeriodCount);

                            RedisConnectionWrapper.RedisStats[connName] = curCount;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
