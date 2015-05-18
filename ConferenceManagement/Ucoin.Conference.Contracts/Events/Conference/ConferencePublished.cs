
namespace Ucoin.Conference.Contracts
{
    using System;
    using Ucoin.Framework.Messaging;

    /// <summary>
    /// Event published whenever a conference is made public.
    /// </summary>
    public class ConferencePublished : IEvent
    {
        public Guid SourceId { get; set; }
    }
}
