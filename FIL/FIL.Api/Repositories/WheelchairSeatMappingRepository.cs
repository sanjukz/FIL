using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IWheelchairSeatMappingRepository : IOrmRepository<WheelchairSeatMapping, WheelchairSeatMapping>
    {
        WheelchairSeatMapping Get(int id);

        IEnumerable<WheelchairSeatMapping> GetByMatchSeatTicketDetails(IEnumerable<long> matchSeatTicketDetailIds);

        WheelchairSeatMapping GetByMatchSeatTicketDetail(long matchSeatTicketDetailId);
    }

    public class WheelchairSeatMappingRepository : BaseOrmRepository<WheelchairSeatMapping>, IWheelchairSeatMappingRepository
    {
        public WheelchairSeatMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public WheelchairSeatMapping Get(int id)
        {
            return Get(new WheelchairSeatMapping { Id = id });
        }

        public IEnumerable<WheelchairSeatMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteWheelChair(WheelchairSeatMapping WheelchairSeat)
        {
            Delete(WheelchairSeat);
        }

        public IEnumerable<WheelchairSeatMapping> GetByMatchSeatTicketDetails(IEnumerable<long> matchSeatTicketDetailIds)
        {
            var matchSeatTicketDetailIdList = GetAll(statement => statement
                                 .Where($"{nameof(WheelchairSeatMapping.MatchSeatTicketDetailId):C} IN @Ids")
                                 .WithParameters(new { Ids = matchSeatTicketDetailIds }));
            return matchSeatTicketDetailIdList;
        }

        public WheelchairSeatMapping GetByMatchSeatTicketDetail(long matchSeatTicketDetailIds)
        {
            var matchSeatTicketDetailIdList = GetAll(statement => statement
                                 .Where($"{nameof(WheelchairSeatMapping.MatchSeatTicketDetailId):C} = @Ids")
                                 .WithParameters(new { Ids = matchSeatTicketDetailIds })).FirstOrDefault();
            return matchSeatTicketDetailIdList;
        }
    }
}