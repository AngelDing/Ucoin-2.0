namespace Ucoin.Conference.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Entities.ViewModel;
    using Ucoin.Framework.Cache;


    public class CachingConferenceService : IConferenceViewService
    {
        private readonly IConferenceViewService decoratedDao;
        private readonly ICacheManager cache;

        public CachingConferenceService(IConferenceViewService decoratedDao, ICacheManager cache)
        {
            this.decoratedDao = decoratedDao;
            this.cache = cache;
        }

        public ConferenceDetails GetConferenceDetails(string conferenceCode)
        {
            var key = "ConferenceDao_Details_" + conferenceCode;

            var cachePolicy = CachePolicy.WithAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(10));

            var conference = this.cache.Get(key, () =>
                {
                    return this.decoratedDao.GetConferenceDetails(conferenceCode);
                },
                cachePolicy);

            return conference;
        }

        public ConferenceAlias GetConferenceAlias(string conferenceCode)
        {
            var key = "ConferenceDao_Alias_" + conferenceCode;

            var cachePolicy = CachePolicy.WithAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(20));

            var conference = this.cache.Get(key, () =>
                {
                    return this.decoratedDao.GetConferenceAlias(conferenceCode);
                },
                cachePolicy);

            return conference;
        }

        public IList<ConferenceAlias> GetPublishedConferences()
        {
            var key = "ConferenceDao_PublishedConferences";

            var cachePolicy = CachePolicy.WithAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(10));

            var cached = this.cache.Get(key, () =>
                {
                   return this.decoratedDao.GetPublishedConferences();
                },
                cachePolicy);

            return cached;
        }

        /// <summary>
        /// Gets ifnromation about the seat types.
        /// </summary>
        public IList<SeatTypeView> GetPublishedSeatTypes(Guid conferenceId)
        {
            var key = "ConferenceDao_PublishedSeatTypes_" + conferenceId;

            var seatTypes = this.cache.Get<IList<SeatTypeView>>(key);

            if (seatTypes == null)
            {
                seatTypes = this.decoratedDao.GetPublishedSeatTypes(conferenceId);
                if (seatTypes != null)
                {
                    // determine how long to cache depending on criticality of using stale data.
                    TimeSpan timeToCache;
                    if (seatTypes.All(x => x.AvailableQuantity > 200 || x.AvailableQuantity <= 0))
                    {
                        timeToCache = TimeSpan.FromMinutes(5);
                    }
                    else if (seatTypes.Any(x => x.AvailableQuantity < 30 && x.AvailableQuantity > 0))
                    {
                        // there are just a few seats remaining. Do not cache.
                        timeToCache = TimeSpan.Zero;
                    }
                    else if (seatTypes.Any(x => x.AvailableQuantity < 100 && x.AvailableQuantity > 0))
                    {
                        timeToCache = TimeSpan.FromSeconds(20);
                    }
                    else
                    {
                        timeToCache = TimeSpan.FromMinutes(1);
                    }

                    if (timeToCache > TimeSpan.Zero)
                    {
                        var cachePolicy = CachePolicy.WithAbsoluteExpiration(DateTimeOffset.UtcNow.Add(timeToCache));
                        this.cache.Set(key, seatTypes, cachePolicy);
                    }
                }
            }

            return seatTypes;
        }

        public IList<SeatTypeName> GetSeatTypeNames(IEnumerable<Guid> seatTypes)
        {
            return this.decoratedDao.GetSeatTypeNames(seatTypes);
        }
    }
}
