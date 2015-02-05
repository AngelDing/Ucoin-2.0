using System;
using Ucoin.Framework.MongoDb;

namespace Ucoin.Log.Services
{
    public class BaseLogService : BaseMongoService
    {
        public void HandleError(Exception ex)
        {
            //TODO:日誌服務自己拋出異常時，可記錄在本地文件中，同時發送郵件給管理員
        }
    }
}
