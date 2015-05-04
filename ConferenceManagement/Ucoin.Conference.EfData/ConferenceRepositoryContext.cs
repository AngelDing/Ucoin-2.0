
using Ucoin.Framework.SqlDb.Repositories;
namespace Ucoin.Conference.EfData
{
    public class ConferenceRepositoryContext : EfRepositoryContext, IConferenceRepositoryContext
    {
        public ConferenceRepositoryContext()
            : base(new ConferenceContext())
        {
        }
    }
}
