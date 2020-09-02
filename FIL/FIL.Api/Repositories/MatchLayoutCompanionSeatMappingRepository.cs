using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMatchLayoutCompanionSeatMappingRepository : IOrmRepository<MatchLayoutCompanionSeatMapping, MatchLayoutCompanionSeatMapping>
    {
        MatchLayoutCompanionSeatMapping Get(int id);

        MatchLayoutCompanionSeatMapping GetByWheelChairSeatId(long wheelChairSeatIds);

        IEnumerable<MatchLayoutCompanionSeatMapping> GetByWheelChairSeatIds(IEnumerable<long> wheelChairSeatIds);
    }

    public class MatchLayoutCompanionSeatMappingRepository : BaseOrmRepository<MatchLayoutCompanionSeatMapping>, IMatchLayoutCompanionSeatMappingRepository
    {
        public MatchLayoutCompanionSeatMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchLayoutCompanionSeatMapping Get(int id)
        {
            return Get(new MatchLayoutCompanionSeatMapping { Id = id });
        }

        public IEnumerable<MatchLayoutCompanionSeatMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<MatchLayoutCompanionSeatMapping> GetByWheelChairSeatIds(IEnumerable<long> wheelChairSeatIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutCompanionSeatMapping.WheelChairSeatId):C}IN @WheelChairSeatId")
                 .WithParameters(new { WheelChairSeatId = wheelChairSeatIds })
             );
        }

        public MatchLayoutCompanionSeatMapping GetByWheelChairSeatId(long wheelChairSeatIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchLayoutCompanionSeatMapping.WheelChairSeatId):C}= @WheelChairSeatId")
                            .WithParameters(new { WheelChairSeatId = wheelChairSeatIds })
                        ).FirstOrDefault();
        }
    }
}