
namespace Ucoin.Log.Entities
{
    public class ErrorLog : BaseLog
    {
        public string ErrorMessages { get; set; }

        public int Priority { get; set; }

        public string MachineName { get; set; }

        public string AppDomainName { get; set; }

        public string ProcessId { get; set; }

        public string ProcessName { get; set; }

        public string Win32ThreadId { get; set; }

        public string ManagedThreadName { get; set; }
    }
}
