
using System;
using Ucoin.Framework.MongoDb.Repositories;

namespace Ucoin.Framework.MongoDb
{
    public abstract class BaseMongoService : IDisposable
    {
        static BaseMongoService()
        {
            MongoInitHelper.InitMongoDBRepository();
        }

        public void Dispose()
        {
            //http://docs.mongodb.org/ecosystem/tutorial/getting-started-with-csharp-driver/#getting-started-with-csharp-driver
            //The C# driver has a connection pool to use connections to the server efficiently. 
            //There is no need to call Connect or Disconnect; 
            //just let the driver take care of the connections (calling Connect is harmless, 
            //but calling Disconnect is bad because it closes all the connections in the connection pool).
        }
    }
}
