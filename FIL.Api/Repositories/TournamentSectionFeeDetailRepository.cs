using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITournamentSectionFeeDetailRepository : IOrmRepository<TournamentSectionFeeDetail, TournamentSectionFeeDetail>
    {
        TournamentSectionFeeDetail Get(int id);
    }

    public class TournamentSectionFeeDetailRepository : BaseOrmRepository<TournamentSectionFeeDetail>, ITournamentSectionFeeDetailRepository
    {
        public TournamentSectionFeeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TournamentSectionFeeDetail Get(int id)
        {
            return Get(new TournamentSectionFeeDetail { Id = id });
        }
    }
}