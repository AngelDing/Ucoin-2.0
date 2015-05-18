using System;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Entities.Registration;
using Ucoin.Framework.SqlDb.Repositories;


namespace Ucoin.Conference.Repositories
{
    public class RegistrationProcessRepository : EfRepository<RegistrationProcess, Guid>, IRegistrationProcessRepository
    {
        private readonly ConferenceContext db;

        public RegistrationProcessRepository(IConferenceRepositoryContext context)
            : base(context)
        {
            this.db = DbContext as ConferenceContext;
        }
    }
}
