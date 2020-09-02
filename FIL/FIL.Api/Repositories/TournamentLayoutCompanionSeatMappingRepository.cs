using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITournamentLayoutCompanionSeatMappingRepository : IOrmRepository<TournamentLayoutCompanionSeatMapping, TournamentLayoutCompanionSeatMapping>
    {
        TournamentLayoutCompanionSeatMapping Get(int id);
    }

    public class TournamentLayoutCompanionSeatMappingRepository : BaseOrmRepository<TournamentLayoutCompanionSeatMapping>, ITournamentLayoutCompanionSeatMappingRepository
    {
        public TournamentLayoutCompanionSeatMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public TournamentLayoutCompanionSeatMapping Get(int id)
        {
            return Get(new TournamentLayoutCompanionSeatMapping { Id = id });
        }
    }
}