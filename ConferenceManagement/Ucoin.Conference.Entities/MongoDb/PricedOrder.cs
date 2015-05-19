namespace Ucoin.Conference.Entities.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Ucoin.Framework.MongoDb.Entities;

    public class PricedOrder : StringKeyMongoEntity
    {
        public PricedOrder()
        {
            this.Lines = new ObservableCollection<PricedOrderLine>();
        }

        public Guid OrderId { get; set; }

        /// <summary>
        /// Used for correlating with the seat assignments.
        /// </summary>
        public Guid? AssignmentsId { get; set; }

        public IList<PricedOrderLine> Lines { get; set; }

        public decimal Total { get; set; }

        public int OrderVersion { get; set; }

        public bool IsFreeOfCharge { get; set; }

        public DateTime? ReservationExpirationDate { get; set; }
    }
}
