using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentHolidayDayRepository : IOrmRepository<ASIMonumentHolidayDay, ASIMonumentHolidayDay>
    {
        ASIMonumentHolidayDay Get(long id);

        IEnumerable<ASIMonumentHolidayDay> GetByASIMonumentId(long asiMonumentId);

        ASIMonumentHolidayDay GetByASIMonumentIdAndHolidayDate(long asiMonumentId, DateTime date);
    }

    public class ASIMonumentHolidayDayRepository : BaseLongOrmRepository<ASIMonumentHolidayDay>, IASIMonumentHolidayDayRepository
    {
        public ASIMonumentHolidayDayRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonumentHolidayDay Get(long id)
        {
            return Get(new ASIMonumentHolidayDay { Id = id });
        }

        public IEnumerable<ASIMonumentHolidayDay> GetByASIMonumentId(long asiMonumentId)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentHolidayDay.ASIMonumentId):C} = @MonumentId")
                .WithParameters(new { MonumentId = asiMonumentId })
            );
        }

        public ASIMonumentHolidayDay GetByASIMonumentIdAndHolidayDate(long asiMonumentId, DateTime date)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentHolidayDay.ASIMonumentId):C} = @MonumentId AND {nameof(ASIMonumentHolidayDay.HolidayDate)} = @Date")
               .WithParameters(new { MonumentId = asiMonumentId, Date = date })
           ).FirstOrDefault();
        }
    }
}