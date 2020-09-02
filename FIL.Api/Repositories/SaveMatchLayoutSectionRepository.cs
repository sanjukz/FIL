using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models.TournamentLayouts;
using System;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories

{
    public interface ISaveMatchLayoutSectionRepository : IOrmRepository<SaveTournamnetSeatLayoutData, SaveTournamnetSeatLayoutData>
    {
        int SaveMatchLayoutSectionData(string sectionIds, int eventId, int venueId, string eventdetailid, Guid createdBy, string feeDetails);
    }

    public class SaveMatchLayoutSectionRepository : BaseOrmRepository<SaveTournamnetSeatLayoutData>, ISaveMatchLayoutSectionRepository
    {
        public SaveMatchLayoutSectionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public int SaveMatchLayoutSectionData(string sectionIds, int eventId, int venueId, string eventdetailid, Guid createdBy, string feeDetails)
        {
            var retvalue = GetCurrentConnection().QueryMultiple("spCopyTournamenttoMatch", new { EventDetailIDs = eventdetailid, SectionXMLData = sectionIds, feeDetailsXmlData = feeDetails, CreatedBy = createdBy },
            commandType: CommandType.StoredProcedure);
            return retvalue.Read<int>().FirstOrDefault();
        }
    }
}