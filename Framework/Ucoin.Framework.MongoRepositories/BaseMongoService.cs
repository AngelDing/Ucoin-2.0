using System;
using Ucoin.Framework.MongoDb.Repositories;

namespace Ucoin.Framework.MongoDb
{
    public abstract class BaseMongoService
    {
        static BaseMongoService()
        {
            MongoInitHelper.InitMongoDBRepository();
        }
    }
}
