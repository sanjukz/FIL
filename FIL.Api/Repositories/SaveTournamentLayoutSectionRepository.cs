using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models.MasterLayout;
using System;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ISaveTournamentLayoutSectionRepository : IOrmRepository<SaveMasterSeatLayoutData, SaveMasterSeatLayoutData>
    {
        int SaveTournamentLayoutSectionData(int eventId, int venueId, string sectionIds, Guid createdBy);

        int SaveSeatLayoutData(string xmlData, int masterVenueLayoutSectionId, bool ShouldSeatInsert);

        int UpdateMatchSeatLayoutData(string xmlData, int masterVenueLayoutSectionId, bool ShouldSeatInsert);
    }

    public class SaveTournamentLayoutSectionRepository : BaseOrmRepository<SaveMasterSeatLayoutData>, ISaveTournamentLayoutSectionRepository
    {
        public SaveTournamentLayoutSectionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public int SaveTournamentLayoutSectionData(int eventId, int venueId, string sectionIds, Guid createdBy)
        {
            var retvalue = GetCurrentConnection().QueryMultiple("spAssignVenueLayoutToTournament", new { EventId = eventId, VenueId = venueId, SectionIds = sectionIds, CreatedBy = createdBy },
            commandType: CommandType.StoredProcedure);
            return retvalue.Read<int>().FirstOrDefault();
        }

        public int SaveSeatLayoutData(string xmlData, int masterVenueLayoutSectionId, bool ShouldSeatInsert)
        {
            if (ShouldSeatInsert)
            {
                var retvalue = GetCurrentConnection().QueryMultiple("spSaveMasterSeatLayout", new { XmlData = xmlData, MasterVenueLayoutSectionId = masterVenueLayoutSectionId, retValue = 0 },
                commandType: CommandType.StoredProcedure);
                return retvalue.Read<int>().FirstOrDefault();
            }
            else
            {
                var retvalue = GetCurrentConnection().QueryMultiple("spUpdateTournamentSeatLayout", new { XmlData = xmlData, MasterVenueLayoutSectionId = masterVenueLayoutSectionId, retValue = 0 },
              commandType: CommandType.StoredProcedure);
                return retvalue.Read<int>().FirstOrDefault();
            }
        }

        public int UpdateMatchSeatLayoutData(string xmlData, int masterVenueLayoutSectionId, bool ShouldSeatInsert)
        {
            if (ShouldSeatInsert)
            {
                var retvalue = GetCurrentConnection().QueryMultiple("spSaveMasterSeatLayout", new { XmlData = xmlData, MasterVenueLayoutSectionId = masterVenueLayoutSectionId, retValue = 0 },
                commandType: CommandType.StoredProcedure);
                return retvalue.Read<int>().FirstOrDefault();
            }
            else
            {
                var retvalue = GetCurrentConnection().QueryMultiple("spUpdateMatchLevelSeatLayout", new { XmlData = xmlData, MatchLayoutSectionId = masterVenueLayoutSectionId, retValue = 0 },
              commandType: CommandType.StoredProcedure);
                return retvalue.Read<int>().FirstOrDefault();
            }
        }
    }
}