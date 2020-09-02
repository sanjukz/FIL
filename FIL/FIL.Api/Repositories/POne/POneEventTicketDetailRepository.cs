using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventTicketDetailRepository : IOrmRepository<POneEventTicketDetail, POneEventTicketDetail>
    {
        POneEventTicketDetail Get(int id);

        POneEventTicketDetail GetByEventDetailIdAndTicketCategoryId(int matchId, int ticketCategoryId);

        IEnumerable<POneEventTicketDetail> GetByPOneEventDetail(int pOneEventDetailId);

        IEnumerable<POneEventTicketDetail> GetByManyPOneEventDetail(List<int> pOneEventDetailIds);
    }

    public class POneEventTicketDetailRepository : BaseOrmRepository<POneEventTicketDetail>, IPOneEventTicketDetailRepository
    {
        public POneEventTicketDetailRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventTicketDetail Get(int id)
        {
            return Get(new POneEventTicketDetail { Id = id });
        }

        public POneEventTicketDetail GetByEventDetailIdAndTicketCategoryId(int matchId, int ticketCategoryId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventTicketDetail.POneEventDetailId):C} = @POneEventDetailId AND {nameof(POneEventTicketDetail.POneTicketCategoryId):C} = @POneTicketCategoryId")
                .WithParameters(new { POneEventDetailId = matchId, POneTicketCategoryId = ticketCategoryId })
            ).FirstOrDefault();
        }

        public IEnumerable<POneEventTicketDetail> GetByPOneEventDetail(int pOneEventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventTicketDetail.POneEventDetailId):C} = @POneEventDetailId")
                .WithParameters(new { POneEventDetailId = pOneEventDetailId })
            );
        }

        public IEnumerable<POneEventTicketDetail> GetByManyPOneEventDetail(List<int> pOneEventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(POneEventTicketDetail.POneEventDetailId):C} IN @POneEventDetailIds")
                .WithParameters(new { POneEventDetailIds = pOneEventDetailIds })
            );
        }
    }
}