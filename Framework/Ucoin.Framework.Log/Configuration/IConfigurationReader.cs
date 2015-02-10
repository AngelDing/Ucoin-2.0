namespace Ucoin.Framework.Logging.Configuration
{
    public  interface IConfigurationReader
    {
        object GetSection(string sectionName);
    }
}
