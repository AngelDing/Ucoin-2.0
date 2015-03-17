using System;

namespace Ucoin.Log.Entities
{
    public interface IPerfLog
    {
        string Unit { get; set; }

        long Ticks { get; set; }

        DateTime TimeStamp { get; set; }
    }
}
