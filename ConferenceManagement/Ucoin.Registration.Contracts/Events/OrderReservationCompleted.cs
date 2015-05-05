﻿
namespace Ucoin.Registration.Contracts
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class OrderReservationCompleted : VersionedEvent
    {
        public DateTime ReservationExpiration { get; set; }

        public IEnumerable<SeatQuantity> Seats { get; set; }
    }
}
