using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    ///<summary>
    ///指定页面的边距尺寸,以百分之一英寸为单位.
    ///</summary>
    [Serializable, DataContract]
    public class MarginEntity
    {
        public MarginEntity() { }

        public MarginEntity(int left, int right, int top, int bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        [DataMember]
        public int Bottom { get; set; }

        [DataMember]
        public int Left { get; set; }

        [DataMember]
        public int Right { get; set; }

        [DataMember]
        public int Top { get; set; }
    }
}
