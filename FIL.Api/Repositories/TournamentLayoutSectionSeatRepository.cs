using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITournamentLayoutSectionSeatRepository : IOrmRepository<TournamentLayoutSectionSeat, TournamentLayoutSectionSeat>
    {
        TournamentLayoutSectionSeat Get(int id);

        int GetSeatCount(int id);

        IEnumerable<TournamentLayoutSectionSeat> GetByTournamentLayoutSectionId(int tournamentLayoutSectionId);
    }

    public class TournamentLayoutSectionSeatRepository : BaseLongOrmRepository<TournamentLayoutSectionSeat>, ITournamentLayoutSectionSeatRepository
    {
        public TournamentLayoutSectionSeatRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TournamentLayoutSectionSeat Get(int id)
        {
            return Get(new TournamentLayoutSectionSeat { Id = id });
        }

        public IEnumerable<TournamentLayoutSectionSeat> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteTournamentLayoutSectionSeat(TournamentLayoutSectionSeat tournamentLayoutSectionSeat)
        {
            Delete(tournamentLayoutSectionSeat);
        }

        public TournamentLayoutSectionSeat SaveTournamentLayoutSectionSeat(TournamentLayoutSectionSeat tournamentLayoutSectionSeat)
        {
            return Save(tournamentLayoutSectionSeat);
        }

        public int GetSeatCount(int id)
        {
            var count = GetCurrentConnection().Query<int>("Select count(*) from TournamentLayoutSectionSeats where TournamentLayoutSectionId=@Id", new
            {
                Id = id
            }).FirstOrDefault();
            return count;
        }

        public IEnumerable<TournamentLayoutSectionSeat> GetByTournamentLayoutSectionId(int tournamentLayoutSectionId)
        {
            return GetAll(s => s.Where($"{nameof(TournamentLayoutSectionSeat.TournamentLayoutSectionId):C}=@TournamentLayoutSectionId")
                  .WithParameters(new { TournamentLayoutSectionId = tournamentLayoutSectionId }));
        }
    }
}