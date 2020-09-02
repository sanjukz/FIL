using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPlaceTicketRedemptionDetailRepository : IOrmRepository<PlaceTicketRedemptionDetail, PlaceTicketRedemptionDetail>
    {
        PlaceTicketRedemptionDetail Get(long id);

        IEnumerable<PlaceTicketRedemptionDetail> GetAllByEventDetailIds(IEnumerable<long> ids);

        IEnumerable<PlaceTicketRedemptionDetail> GetAllByEventDetailId(long id);
    }

    public class PlaceTicketRedemptionDetailRepository : BaseLongOrmRepository<PlaceTicketRedemptionDetail>, IPlaceTicketRedemptionDetailRepository
    {
        public PlaceTicketRedemptionDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PlaceTicketRedemptionDetail Get(long id)
        {
            return Get(new PlaceTicketRedemptionDetail { Id = id });
        }

        public IEnumerable<PlaceTicketRedemptionDetail> GetAllByEventDetailIds(IEnumerable<long> ids)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(PlaceTicketRedemptionDetail.EventDetailId):C} IN @Ids")
                .WithParameters(new { Ids = ids }));
            return eventTicketDetailList;
        }

        public IEnumerable<PlaceTicketRedemptionDetail> GetAllByEventDetailId(long id)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(PlaceTicketRedemptionDetail.EventDetailId):C} = @Ids")
                .WithParameters(new { Ids = id }));
            return eventTicketDetailList;
        }
    }
}