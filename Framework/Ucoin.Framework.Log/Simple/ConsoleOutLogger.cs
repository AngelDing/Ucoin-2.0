using System;
using System.Collections.Generic;
using System.Text;

namespace Ucoin.Framework.Logging.Simple
{
    public class ConsoleOutLogger : BaseSimpleLogger
    {
        private static readonly Dictionary<LogLevel, ConsoleColor> colors =
            new Dictionary<LogLevel, ConsoleColor>
            {
                { LogLevel.Fatal, ConsoleColor.Red },
                { LogLevel.Error, ConsoleColor.Yellow },
                { LogLevel.Warn, ConsoleColor.Magenta },
                { LogLevel.Info, ConsoleColor.White },
                { LogLevel.Debug, ConsoleColor.Gray },
                { LogLevel.Trace, ConsoleColor.DarkGray },
            };

        private readonly bool useColor;

        public ConsoleOutLogger(LogArgumentEntity argEntity)
            : base(argEntity)
        {
        }

        public ConsoleOutLogger(LogArgumentEntity argEntity, bool useColor)
            : this(argEntity)
        {
            this.useColor = useColor;
        }

        protected override void Write(LogLevel level, object message, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            FormatOutput(sb, level, message, ex);

            ConsoleColor color;
            if (this.useColor && colors.TryGetValue(level, out color))
            {
                var originalColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = color;
                    Console.Out.WriteLine(sb.ToString());
                    return;
                }
                finally
                {
                    Console.ForegroundColor = originalColor;
                }
            }

            Console.Out.WriteLine(sb.ToString());
        } 
    }
}
