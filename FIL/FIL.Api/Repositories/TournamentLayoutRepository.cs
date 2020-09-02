using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITournamentLayoutRepository : IOrmRepository<TournamentLayout, TournamentLayout>
    {
        TournamentLayout Get(int id);

        IEnumerable<TournamentLayout> GetTournamentLayout(IEnumerable<int> masterVenueLayoutId);

        IEnumerable<TournamentLayout> GetAllByTournamentId(int id);

        IEnumerable<TournamentLayout> GetByMasterLayoutAndEventId(int masterVenueLayoutId, int eventId);
    }

    public class TournamentLayoutRepository : BaseOrmRepository<TournamentLayout>, ITournamentLayoutRepository
    {
        public TournamentLayoutRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TournamentLayout Get(int id)
        {
            return Get(new TournamentLayout { Id = id });
        }

        public IEnumerable<TournamentLayout> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteTournamentLayout(TournamentLayout tournamentLayout)
        {
            Delete(tournamentLayout);
        }

        public TournamentLayout SaveTournamentLayout(TournamentLayout tournamentLayout)
        {
            return Save(tournamentLayout);
        }

        public IEnumerable<TournamentLayout> GetTournamentLayout(IEnumerable<int> masterVenueLayoutId)
        {
            return GetAll(s => s.Where($"{nameof(TournamentLayout.MasterVenueLayoutId):C} IN @Id")
                .WithParameters(new { Id = masterVenueLayoutId })
            );
        }

        public IEnumerable<TournamentLayout> GetAllByTournamentId(int id)
        {
            return GetAll(s => s.Where($"{nameof(TournamentLayout.Id):C}=@Id")
                  .WithParameters(new { Id = id }));
        }

        public IEnumerable<TournamentLayout> GetByMasterLayoutAndEventId(int masterVenueLayoutId, int eventId)
        {
            return GetAll(statement => statement
                           .Where($"{nameof(TournamentLayout.MasterVenueLayoutId):C} = @MasterVenueLayoutId And {nameof(TournamentLayout.EventId):C} = @EventId")
                           .WithParameters(new { MasterVenueLayoutId = masterVenueLayoutId, EventId = eventId }));
        }
    }
}