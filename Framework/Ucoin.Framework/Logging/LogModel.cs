using Ucoin.Framework.Models;

namespace Ucoin.Framework.Logging
{
    public class LogModel : IModel
    {
        public string Message { get; set; }

        public string Detail { get; set; }

        public AppCodeType AppCodeType { get; set; }

        public string Source { get; set; }
    }
}
