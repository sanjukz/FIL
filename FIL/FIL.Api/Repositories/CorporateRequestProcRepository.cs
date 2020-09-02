using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Data;

namespace FIL.Api.Repositories
{
    public interface ICorporateRequestProcRepository : IOrmRepository<CorporateRequestProcData, CorporateRequestProcData>
    {
        CorporateRequestProcData GetEventDetailData();
    }

    public class CorporateRequestProcRepository : BaseOrmRepository<CorporateRequestProcData>, ICorporateRequestProcRepository
    {
        public CorporateRequestProcRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateRequestProcData GetEventDetailData()
        {
            CorporateRequestProcData corporateRequestProcData = new CorporateRequestProcData();
            var corporateRequestEventdata = GetCurrentConnection().QueryMultiple("CorporateRequest", new { }, commandType: CommandType.StoredProcedure);
            corporateRequestProcData.EventDetail = corporateRequestEventdata.Read<EventDetail>();
            corporateRequestProcData.Event = corporateRequestEventdata.Read<Event>();
            return corporateRequestProcData;
        }
    }
}