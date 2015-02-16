namespace Ucoin.Framework.Logging.EntLib
{
    public class LoggerSettings
    {
        public static readonly int DEFAULTPRIORITY = -1;

        public static readonly string DEFAULTEXCEPTIONFORMAT = "Exception[ message = $(exception.message), source = $(exception.source), targetsite = $(exception.targetsite), stacktrace = $(exception.stacktrace) ]";

        public readonly int priority = DEFAULTPRIORITY;

        public readonly string exceptionFormat = DEFAULTEXCEPTIONFORMAT;

        public LoggerSettings(int defaultPriority, string exceptionFormat)
        {
            this.priority = defaultPriority;
            this.exceptionFormat = exceptionFormat;
        }
    }
}