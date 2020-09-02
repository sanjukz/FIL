using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITeamRepository : IOrmRepository<Team, Team>
    {
        Team Get(long id);
    }

    public class TeamRepository : BaseLongOrmRepository<Team>, ITeamRepository
    {
        public TeamRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Team Get(long id)
        {
            return Get(new Team { Id = id });
        }

        public IEnumerable<Team> GetAll()
        {
            return GetAll(null);
        }
    }
}