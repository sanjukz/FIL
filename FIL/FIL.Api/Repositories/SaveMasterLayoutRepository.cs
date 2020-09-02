using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models.MasterLayout;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ISaveMasterLayoutRepository : IOrmRepository<SaveMasterSeatLayoutData, SaveMasterSeatLayoutData>
    {
        int SaveSeatLayoutData(string xmlData, int masterVenueLayoutSectionId, bool ShouldSeatInsert);
    }

    public class SaveMasterLayoutRepository : BaseOrmRepository<SaveMasterSeatLayoutData>, ISaveMasterLayoutRepository
    {
        public SaveMasterLayoutRepository(IDataSettings dataSettings) : base(dataSettings)
        {
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
                var retvalue = GetCurrentConnection().QueryMultiple("spUpdateMasterSeatLayout", new { XmlData = xmlData, MasterVenueLayoutSectionId = masterVenueLayoutSectionId, retValue = 0 },
              commandType: CommandType.StoredProcedure);
                return retvalue.Read<int>().FirstOrDefault();
            }
        }
    }
}