namespace CtripSZ.ServiceModel.Runtime
{
    public interface IServiceFilter
    {
        
        bool IsEmpty { get; }
        
        bool Pass(IService test);
        
        bool Match(IService test);
    }
}
