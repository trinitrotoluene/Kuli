using Serilog.Events;

namespace Kuli.Configuration
{
    public sealed class LoggingOptions
    {
        public const string Section = "logging";

        public LogEventLevel Level { get; set; } = LogEventLevel.Debug;
    }
}