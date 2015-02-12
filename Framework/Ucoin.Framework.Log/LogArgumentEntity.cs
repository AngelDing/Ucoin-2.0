
namespace Ucoin.Framework.Logging
{
    public class LogArgumentEntity
    {
        public string LogName { get; set; }

        public LogLevel Level { get; set; }

        public bool ShowLevel { get; set; }

        public bool ShowDateTime { get; set; }

        public bool ShowLogName { get; set; }

        public string DateTimeFormat { get; set; }

        public bool HasDateTimeFormat
        {
            get
            {
                return !string.IsNullOrEmpty(this.DateTimeFormat);
            }
        }
    } 
}
