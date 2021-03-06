﻿
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class OrderPartiallyReserved : VersionedEvent
    {
        public DateTime ReservationExpiration { get; set; }

        public IEnumerable<SeatQuantity> Seats { get; set; }
    }
}
