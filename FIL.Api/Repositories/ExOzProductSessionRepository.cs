using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzProductSessionRepository : IOrmRepository<ExOzProductSession, ExOzProductSession>
    {
        ExOzProductSession Get(long id);

        List<ExOzProductSession> DisableAllExOzProductSessions();

        IEnumerable<ExOzProductSession> GetBySessionIds(List<long> sessionIds);

        ExOzProductSession GetBySessionId(long sessionId);
    }

    public class ExOzProductSessionRepository : BaseLongOrmRepository<ExOzProductSession>, IExOzProductSessionRepository
    {
        public ExOzProductSessionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzProductSession Get(long id)
        {
            return Get(new ExOzProductSession { Id = id });
        }

        public IEnumerable<ExOzProductSession> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteProductSession(ExOzProductSession exOzProductSession)
        {
            Delete(exOzProductSession);
        }

        public ExOzProductSession SaveProductSession(ExOzProductSession exOzProductSession)
        {
            return Save(exOzProductSession);
        }

        public List<ExOzProductSession> DisableAllExOzProductSessions()
        {
            List<ExOzProductSession> allExOzProductSessions = this.GetAll().ToList();
            foreach (var op in allExOzProductSessions)
            {
                op.IsEnabled = false;
                Save(op);
            }
            return allExOzProductSessions;
        }

        public IEnumerable<ExOzProductSession> GetBySessionIds(List<long> sessionIds)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductSession.ProductSessionId):C}IN @SessionIds")
                 .WithParameters(new { SessionIds = sessionIds })
             );
        }

        public ExOzProductSession GetBySessionId(long sessionId)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductSession.ProductSessionId):C} = @SessionId")
                .WithParameters(new { SessionId = sessionId })
            ).FirstOrDefault();
        }
    }
}