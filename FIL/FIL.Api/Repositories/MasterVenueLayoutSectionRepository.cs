using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMasterVenueLayoutSectionRepository : IOrmRepository<MasterVenueLayoutSection, MasterVenueLayoutSection>
    {
        MasterVenueLayoutSection Get(int id);

        IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutIdandMasterVenueLayoutSectionIdandVenueLayoutAreaId(int masterVenueLayoutId, int masterVenueLayoutSectionId, int venueLayoutAreaId);

        IEnumerable<MasterVenueLayoutSection> GetByVenueLayoutAreaId(int venueLayoutAreaId);

        IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutIdandVenueLayoutAreaId(int masterVenueLayoutId, int venueLayoutAreaId);

        IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutSectionIdandVenueLayoutAreaId(int masterVenueLayoutSectionId, int venueLayoutAreaId);

        List<SectionDetailsByVenueLayout> SectionDetailsByVenueLayout(int masterVenueLayoutId);

        MasterVenueLayoutSection GetByAltId(Guid AltId);

        IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutId(int masterVenueLayoutId);

        IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutIdAndVenueAreaId(int masterVenueLayoutId, int venueAreaId);

        IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutSectionIdAndVenueAreaId(int masterVenueLayoutSectionId, int venueAreaId);

        int GetExistingChildCapacity(int masterVenueLayoutSectionId);

        int GetExistingChildCapacityAtTournament(int TournamentLayoutSectionId);

        List<TournamentSectionDetailsByVenueLayout> TournamentSectionDetailsByVenueLayout(int masterVenueLayout, int eventId);

        List<TournamentSectionDetailsByVenueLayout> TournamentLayoutSectionsByTournamentLayoutId(int tournamentLayoutId, int eventId);
    }

    public class MasterVenueLayoutSectionRepository : BaseOrmRepository<MasterVenueLayoutSection>, IMasterVenueLayoutSectionRepository
    {
        public MasterVenueLayoutSectionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterVenueLayoutSection Get(int id)
        {
            return Get(new MasterVenueLayoutSection { Id = id });
        }

        public IEnumerable<MasterVenueLayoutSection> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMasterVenueLayoutSection(MasterVenueLayoutSection masterVenueLayoutSection)
        {
            Delete(masterVenueLayoutSection);
        }

        public MasterVenueLayoutSection SaveMasterVenueLayoutSection(MasterVenueLayoutSection masterVenueLayoutSection)
        {
            return Save(masterVenueLayoutSection);
        }

        public IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutIdandMasterVenueLayoutSectionIdandVenueLayoutAreaId(int masterVenueLayoutId, int masterVenueLayoutSectionId, int venueLayoutAreaId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSection.MasterVenueLayoutId):C}= @MasterVenueLayoutId AND {nameof(MasterVenueLayoutSection.MasterVenueLayoutSectionId):C}= @MasterVenueLayoutSectionId AND {nameof(MasterVenueLayoutSection.VenueLayoutAreaId):C}= @VenueLayoutAreaId")
           .WithParameters(new { MasterVenueLayoutId = masterVenueLayoutId, MasterVenueLayoutSectionId = masterVenueLayoutSectionId, VenueLayoutAreaId = venueLayoutAreaId })
           );
        }

        public IEnumerable<MasterVenueLayoutSection> GetByVenueLayoutAreaId(int venueLayoutAreaId)
        {
            return GetAll(statement => statement.Where($"{nameof(MasterVenueLayoutSection.VenueLayoutAreaId):C} = @venuelayoutAreaId")
                         .WithParameters(new { venuelayoutAreaId = venueLayoutAreaId }));
        }

        public MasterVenueLayoutSection GetByAltId(Guid AltId)
        {
            return GetAll(statement => statement.Where($"{nameof(MasterVenueLayoutSection.AltId):C} = @altId")
                         .WithParameters(new { altId = AltId })).FirstOrDefault();
        }

        public IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutIdandVenueLayoutAreaId(int masterVenueLayoutId, int venueLayoutAreaId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSection.MasterVenueLayoutId):C}= @MasterVenueLayoutId  AND {nameof(MasterVenueLayoutSection.VenueLayoutAreaId):C}= @VenueLayoutAreaId")
           .WithParameters(new { MasterVenueLayoutId = masterVenueLayoutId, VenueLayoutAreaId = venueLayoutAreaId })
           );
        }

        public IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutSectionIdandVenueLayoutAreaId(int masterVenueLayoutSectionId, int venueLayoutAreaId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSection.MasterVenueLayoutSectionId):C}= @MasterVenueLayoutSectionId  AND {nameof(MasterVenueLayoutSection.VenueLayoutAreaId):C}= @VenueLayoutAreaId")
           .WithParameters(new { MasterVenueLayoutSectionId = masterVenueLayoutSectionId, VenueLayoutAreaId = venueLayoutAreaId })
           );
        }

        public IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutId(int masterVenueLayoutId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSection.MasterVenueLayoutId):C}=@Id")
                  .WithParameters(new { Id = masterVenueLayoutId }));
        }

        public IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutIdAndVenueAreaId(int masterVenueLayoutId, int venueAreaId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSection.VenueLayoutAreaId):C}=@VenueLayoutAreaId AND {nameof(MasterVenueLayoutSection.MasterVenueLayoutId):C}=@MasterVenueLayoutId")
           .WithParameters(new { MasterVenueLayoutId = masterVenueLayoutId, VenueLayoutAreaId = venueAreaId }));
        }

        public IEnumerable<MasterVenueLayoutSection> GetByMasterVenueLayoutSectionIdAndVenueAreaId(int masterVenueLayoutSectionId, int venueAreaId)
        {
            return GetAll(s => s.Where($"{nameof(MasterVenueLayoutSection.VenueLayoutAreaId):C}=@VenueLayoutAreaId AND {nameof(MasterVenueLayoutSection.MasterVenueLayoutSectionId):C}=@MasterVenueLayoutSectionId")
           .WithParameters(new { MasterVenueLayoutSectionId = masterVenueLayoutSectionId, VenueLayoutAreaId = venueAreaId }));
        }

        public List<SectionDetailsByVenueLayout> SectionDetailsByVenueLayout(int masterVenueLayoutId)
        {
            //List<SectionDetailsByVenueLayout> sectionDetailsByVenueLayout = new List<SectionDetailsByVenueLayout>();
            var data = GetCurrentConnection().QueryMultiple("spMasterVenueStandDetails", new { LayoutId = masterVenueLayoutId }, commandType: CommandType.StoredProcedure);
            return data.Read<SectionDetailsByVenueLayout>().ToList();
        }

        public List<TournamentSectionDetailsByVenueLayout> TournamentSectionDetailsByVenueLayout(int masterVenueLayoutId, int eventId)
        {
            //List<SectionDetailsByVenueLayout> sectionDetailsByVenueLayout = new List<SectionDetailsByVenueLayout>();
            var data = GetCurrentConnection().QueryMultiple("spTournamentLevelStandDetails", new { LayoutId = masterVenueLayoutId, EventId = eventId }, commandType: CommandType.StoredProcedure);
            return data.Read<TournamentSectionDetailsByVenueLayout>().ToList();
        }

        public int GetExistingChildCapacity(int masterVenueLayoutSectionId)
        {
            var count = GetCurrentConnection().Query<int>("select sum(Capacity) from MasterVenueLayoutSections where MasterVenueLayoutSectionId=@MasterVenueLayoutSectionId group by MasterVenueLayoutSectionId  ", new
            {
                MasterVenueLayoutSectionId = masterVenueLayoutSectionId
            }).FirstOrDefault();
            return count;
        }

        public List<TournamentSectionDetailsByVenueLayout> TournamentLayoutSectionsByTournamentLayoutId(int tournamentLayoutId, int eventId)
        {
            //List<SectionDetailsByVenueLayout> sectionDetailsByVenueLayout = new List<SectionDetailsByVenueLayout>();
            var data = GetCurrentConnection().QueryMultiple("spTournamentLayoutSectionsByTournamentLayoutId", new { LayoutId = tournamentLayoutId, EventId = eventId }, commandType: CommandType.StoredProcedure);
            return data.Read<TournamentSectionDetailsByVenueLayout>().ToList();
        }

        public int GetExistingChildCapacityAtTournament(int tournamentLayoutSectionId)
        {
            var count = GetCurrentConnection().Query<int>("select sum(Capacity) from TournamentLayoutSections where TournamentLayoutSectionId=@TournamentLayoutSectionId group by TournamentLayoutSectionId  ", new
            {
                TournamentLayoutSectionId = tournamentLayoutSectionId
            }).FirstOrDefault();
            return count;
        }
    }
}