using Ucoin.Framework.Models;

namespace Ucoin.Framework.Logging
{
    public class LogModel : IModel
    {
        public LogModel()
        {
            LogLevelType = LogLevelType.All;
        }

        public string Message { get; set; }        

        public AppCodeType AppCodeType { get; set; }

        public LogLevelType LogLevelType { get; set; }

        public string Detail { get; set; } 

        public string Source { get; set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
