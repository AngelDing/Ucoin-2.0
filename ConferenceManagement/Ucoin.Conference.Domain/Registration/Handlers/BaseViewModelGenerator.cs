
using Ucoin.Framework.MongoDb.Repositories;

namespace Ucoin.Conference.Domain
{
    public class BaseViewModelGenerator
    {
        static BaseViewModelGenerator()
        {
            MongoInitHelper.InitMongoDBRepository();
        }
    }
}
