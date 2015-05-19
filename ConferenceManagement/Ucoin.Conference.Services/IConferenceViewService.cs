namespace Ucoin.Conference.Services
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Entities.ViewModel;

    public interface IConferenceViewService
    {
        ConferenceDetails GetConferenceDetails(string conferenceCode);

        ConferenceAlias GetConferenceAlias(string conferenceCode);

        IList<ConferenceAlias> GetPublishedConferences();

        IList<SeatTypeView> GetPublishedSeatTypes(Guid conferenceId);

        IList<SeatTypeName> GetSeatTypeNames(IEnumerable<Guid> seatTypes);
    }
}