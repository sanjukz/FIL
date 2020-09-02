using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IMatchLayoutRepository : IOrmRepository<MatchLayout, MatchLayout>
    {
        MatchLayout Get(int id);

        IEnumerable<MatchLayout> GetByTournamentLayoutId(int tournamentLayoutId);
    }

    public class MatchLayoutRepository : BaseOrmRepository<MatchLayout>, IMatchLayoutRepository
    {
        public MatchLayoutRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchLayout Get(int id)
        {
            return Get(new MatchLayout { Id = id });
        }

        public IEnumerable<MatchLayout> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMatchLayout(MatchLayout matchLayout)
        {
            Delete(matchLayout);
        }

        public MatchLayout SaveMatchLayout(MatchLayout matchLayout)
        {
            return Save(matchLayout);
        }

        public IEnumerable<MatchLayout> GetByTournamentLayoutId(int tournamentLayoutId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(MatchLayout.TournamentLayoutId):C} = @TournamentLayoutId")
                    .WithParameters(new { TournamentLayoutId = tournamentLayoutId }));
        }
    }
}