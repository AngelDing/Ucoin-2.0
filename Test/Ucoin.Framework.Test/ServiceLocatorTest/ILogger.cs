namespace Ucoin.Framework.ServiceLocator.Test
{
    public interface ILogger
    {
        void Log(string msg);
    }

    public interface IUnregisteredLogger
    {
        void Log(string msg);
    }
}