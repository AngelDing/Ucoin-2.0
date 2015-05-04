using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Entities;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public class ConferenceRepository : EfRepository<ConferenceInfo, Guid>, IConferenceRepository
    {
        private readonly ConferenceContext db;
        public ConferenceRepository(IConferenceRepositoryContext context)
            : base(context)
        {
            this.db = DbContext as ConferenceContext;
        }

        public IEnumerable<SeatType> FindSeatTypes(Guid conferenceId)
        {
            IEnumerable<SeatType> seatTypes;
            seatTypes = this.RetryPolicy.ExecuteAction(() => db.Conferences.Include(x => x.Seats)
                    .Where(x => x.Id == conferenceId)
                    .Select(x => x.Seats)
                    .FirstOrDefault());
            if (seatTypes == null)
            {
                seatTypes = Enumerable.Empty<SeatType>();
            }

            return seatTypes;
        }
    }
}
