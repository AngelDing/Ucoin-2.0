
namespace Ucoin.Framework.Serialization
{
    public enum SerializationFormat
    {
        /// <summary>
        /// No serialization format set 
        /// </summary>
        None = 0,

        /// <summary>
        /// Null serailization
        /// </summary>
        Null,

        /// <summary>
        /// JSON serialization
        /// </summary>
        Json,

        /// <summary>
        /// XML serialization 
        /// </summary>
        Xml,

        /// <summary>
        /// Binary serialization
        /// </summary>
        Binary,

        /// <summary>
        /// Jil serialization
        /// </summary>
        Jil 
    }
}
