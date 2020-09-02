using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentWeekOpenDayRepository : IOrmRepository<ASIMonumentWeekOpenDay, ASIMonumentWeekOpenDay>
    {
        ASIMonumentWeekOpenDay Get(long id);

        IEnumerable<ASIMonumentWeekOpenDay> GetByASIMonumentId(long asiMonumentId);

        ASIMonumentWeekOpenDay GetByASIMonumentIdAndDayId(long asiMonumentId, long dayId);
    }

    public class ASIMonumentWeekOpenDayRepository : BaseLongOrmRepository<ASIMonumentWeekOpenDay>, IASIMonumentWeekOpenDayRepository
    {
        public ASIMonumentWeekOpenDayRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonumentWeekOpenDay Get(long id)
        {
            return Get(new ASIMonumentWeekOpenDay { Id = id });
        }

        public IEnumerable<ASIMonumentWeekOpenDay> GetByASIMonumentId(long asiMonumentId)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentWeekOpenDay.ASIMonumentId):C} = @MonumentId")
                .WithParameters(new { MonumentId = asiMonumentId })
            );
        }

        public ASIMonumentWeekOpenDay GetByASIMonumentIdAndDayId(long asiMonumentId, long dayId)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentWeekOpenDay.ASIMonumentId):C} = @MonumentId AND {nameof(ASIMonumentWeekOpenDay.DayId)} = @DayId")
               .WithParameters(new { MonumentId = asiMonumentId, DayId = dayId })
           ).FirstOrDefault();
        }
    }
}