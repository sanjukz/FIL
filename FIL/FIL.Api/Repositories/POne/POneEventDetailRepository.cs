using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventDetailRepository : IOrmRepository<POneEventDetail, POneEventDetail>
    {
        POneEventDetail Get(int id);

        POneEventDetail GetByName(string name);

        IEnumerable<POneEventDetail> GetAllByPOneEventId(int pOneEventId);

        POneEventDetail GetByPOneId(int pOneId);
    }

    public class POneEventDetailRepository : BaseOrmRepository<POneEventDetail>, IPOneEventDetailRepository
    {
        public POneEventDetailRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventDetail Get(int id)
        {
            return Get(new POneEventDetail { Id = id });
        }

        public POneEventDetail GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetail.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public IEnumerable<POneEventDetail> GetAllByPOneEventId(int pOneEventId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetail.POneEventId):C} = @POneEventId")
                .WithParameters(new { POneEventId = pOneEventId })
            );
        }

        public POneEventDetail GetByPOneId(int pOneId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetail.POneId):C} = @POneId")
                .WithParameters(new { POneId = pOneId })
            ).FirstOrDefault();
        }
    }
}