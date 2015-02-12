
using System;
using System.Text;

namespace Ucoin.Framework.Logging.Simple
{
    public class DebugOutLogger : BaseSimpleLogger
    {
        public DebugOutLogger(LogArgumentEntity argEntity)
            : base(argEntity)
        {
        } 

        protected override void Write(LogLevel level, object message, Exception e)
        {
            var sb = new StringBuilder();
            FormatOutput(sb, level, message, e);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }
    }
}
