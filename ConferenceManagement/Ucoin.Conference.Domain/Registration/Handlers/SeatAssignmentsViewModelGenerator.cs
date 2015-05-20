
namespace Ucoin.Conference.Domain.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using Ucoin.Framework.Messaging.Handling;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Repositories;

    public class SeatAssignmentsViewModelGenerator :
        IEventHandler<SeatAssignmentsCreated>,
        IEventHandler<SeatAssigned>,
        IEventHandler<SeatUnassigned>,
        IEventHandler<SeatAssignmentUpdated>
    {
        private readonly ConferenceMongoRepository<OrderSeats> orderSeatsRepository;
        public SeatAssignmentsViewModelGenerator()
        {
            orderSeatsRepository = new ConferenceMongoRepository<OrderSeats>();
        }

        static SeatAssignmentsViewModelGenerator()
        {
            Mapper.CreateMap<SeatAssigned, OrderSeat>();
            Mapper.CreateMap<SeatAssignmentUpdated, OrderSeat>();
        }

        public void Handle(SeatAssignmentsCreated @event)
        {
            //var seatTypes = this.conferenceDao.GetSeatTypeNames(@event.Seats.Select(x => x.SeatType))
            //    .ToDictionary(x => x.Id, x => x.Name);

            //var dto = new OrderSeats(@event.SourceId, @event.OrderId, @event.Seats.Select(i =>
            //    new OrderSeat(i.Position, seatTypes.TryGetValue(i.SeatType))));

            //orderSeatsRepository.Update(dto);
        }

        public void Handle(SeatAssigned @event)
        {
            var dto = Find(@event.SourceId);
            var seat = dto.Seats.First(x => x.Position == @event.Position);
            Mapper.Map(@event, seat);
            orderSeatsRepository.Update(dto);
        }

        public void Handle(SeatUnassigned @event)
        {
            var dto = Find(@event.SourceId);
            var seat = dto.Seats.First(x => x.Position == @event.Position);
            seat.Attendee.Email = seat.Attendee.FirstName = seat.Attendee.LastName = null;
            orderSeatsRepository.Update(dto);
        }

        public void Handle(SeatAssignmentUpdated @event)
        {
            var dto = Find(@event.SourceId);
            var seat = dto.Seats.First(x => x.Position == @event.Position);
            Mapper.Map(@event, seat);
            orderSeatsRepository.Update(dto);
        }

        private OrderSeats Find(Guid id)
        {
            var dto = orderSeatsRepository.GetBy(o => o.OrderId == id).FirstOrDefault();
            return dto;
        }
    }
}
