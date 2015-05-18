using System;
using Ucoin.Conference.Entities;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public interface IRegistrationProcessRepository : IEfRepository<RegistrationProcess, Guid>
    {
    }
}
