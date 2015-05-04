using System;
using System.Collections.Generic;
using Ucoin.Conference.Contracts;
using Ucoin.Conference.Entities;

namespace Ucoin.Conference.Services
{
    public interface IConferenceService
    {
        void CreateConference(ConferenceInfo conference);

        void UpdateConference(ConferenceInfo conference);

        void CreateSeat(Guid conferenceId, SeatType seat);

        void UpdateSeat(Guid conferenceId, SeatType seat);

        ConferenceInfo FindConference(string slug);

        ConferenceInfo FindConference(string email, string accessCode);

        IEnumerable<SeatType> FindSeatTypes(Guid conferenceId);

        SeatType FindSeatType(Guid seatTypeId);

        IEnumerable<Order> FindOrders(Guid conferenceId);

        void Publish(Guid conferenceId);

        void Unpublish(Guid conferenceId);

        void DeleteSeat(Guid id);
    }
}
