namespace Ucoin.Log.Entities
{
    public class AppPerfLog : SysPerfLog
    {
        public string ControllerName { get; set; }

        public string ActionName { get; set; }        
    }
}
