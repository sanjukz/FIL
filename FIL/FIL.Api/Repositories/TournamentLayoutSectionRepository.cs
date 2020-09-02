using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITournamentLayoutSectionRepository : IOrmRepository<TournamentLayoutSection, TournamentLayoutSection>
    {
        TournamentLayoutSection Get(int id);

        List<SectionDetailsByTournamentLayout> SectionDetailsByTournamentLayout(int layoutId, int eventDetailId);

        IEnumerable<TournamentLayoutSection> GetByMasterVenueLayoutSectionId(IEnumerable<int> masterVenueLayoutSectionIds);

        TournamentLayoutSection GetByMasterVenueLayoutSectionIdAndTournamentLayoutId(int masterVenueLayoutSectionId, int tournamentLayoutId);

        List<TournamentSectionDetailsByVenueLayout> TournamentLayoutSectionsByTournamentLayoutId(int tournamentLayoutId, int eventId);

        IEnumerable<TournamentLayoutSection> GetTournamentLayoutSectionId(int tournamentLayoutSectionId);
    }

    public class TournamentLayoutSectionRepository : BaseOrmRepository<TournamentLayoutSection>, ITournamentLayoutSectionRepository
    {
        public TournamentLayoutSectionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TournamentLayoutSection Get(int id)
        {
            return Get(new TournamentLayoutSection { Id = id });
        }

        public IEnumerable<TournamentLayoutSection> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteTournamentLayoutSection(TournamentLayoutSection tournamentLayoutSection)
        {
            Delete(tournamentLayoutSection);
        }

        public TournamentLayoutSection SaveTournamentLayoutSection(TournamentLayoutSection tournamentLayoutSection)
        {
            return Save(tournamentLayoutSection);
        }

        public List<SectionDetailsByTournamentLayout> SectionDetailsByTournamentLayout(int layoutId, int eventDetailId)
        {
            var data = GetCurrentConnection().QueryMultiple("spTournamentLayoutSectionsByTournamentLayoutIdAtMatchLevel", new { LayoutId = layoutId, EventDetailId = eventDetailId }, commandType: CommandType.StoredProcedure);
            return data.Read<SectionDetailsByTournamentLayout>().ToList();
        }

        public IEnumerable<TournamentLayoutSection> GetByMasterVenueLayoutSectionId(IEnumerable<int> masterVenueLayoutSectionIds)
        {
            return GetAll(s => s.Where($"{nameof(TournamentLayoutSection.MasterVenueLayoutSectionId):C}In @MasterVenueLayoutSectionId ")
               .WithParameters(new { MasterVenueLayoutSectionId = masterVenueLayoutSectionIds })
               );
        }

        public TournamentLayoutSection GetByMasterVenueLayoutSectionIdAndTournamentLayoutId(int masterVenueLayoutSectionId, int tournamentLayoutId)
        {
            return GetAll(s => s.Where($"{nameof(TournamentLayoutSection.TournamentLayoutId):C}= @TournamentLayoutId And {nameof(TournamentLayoutSection.MasterVenueLayoutSectionId):C}= @MasterVenueLayoutSectionId ")
               .WithParameters(new { TournamentLayoutId = tournamentLayoutId, MasterVenueLayoutSectionId = masterVenueLayoutSectionId })
               ).FirstOrDefault();
        }

        public IEnumerable<TournamentLayoutSection> GetTournamentLayoutSectionId(int tournamentLayoutSectionId)
        {
            return GetAll(s => s.Where($"{nameof(TournamentLayoutSection.TournamentLayoutSectionId):C}= @TournamentLayoutSectionId ")
               .WithParameters(new { TournamentLayoutSectionId = tournamentLayoutSectionId })
               );
        }

        public List<TournamentSectionDetailsByVenueLayout> TournamentLayoutSectionsByTournamentLayoutId(int tournamentLayoutId, int eventId)
        {
            //List<SectionDetailsByVenueLayout> sectionDetailsByVenueLayout = new List<SectionDetailsByVenueLayout>();
            var data = GetCurrentConnection().QueryMultiple("spTournamentLayoutSectionsByTournamentLayoutId", new { LayoutId = tournamentLayoutId, EventId = eventId }, commandType: CommandType.StoredProcedure);
            return data.Read<TournamentSectionDetailsByVenueLayout>().ToList();
        }
    }
}