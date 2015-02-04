using System.Runtime.Serialization;

namespace Ucoin.ServiceModel.Server.Runtime
{
    [DataContract]
    public enum RunState
    {
        [EnumMember]
        NotRunnable,

        [EnumMember]
        Runnable,

        [EnumMember]
        Error
    }
}
