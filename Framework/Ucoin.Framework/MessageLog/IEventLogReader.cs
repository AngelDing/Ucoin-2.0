
namespace Ucoin.Framework.MessageLog
{
    using System.Collections.Generic;
    using Ucoin.Framework.Messaging;

    /// <summary>
    /// Exposes the message log for all events that the system processed.
    /// </summary>
    public interface IEventLogReader
    {
        IEnumerable<IEvent> Query(QueryCriteria criteria);
    }
}
