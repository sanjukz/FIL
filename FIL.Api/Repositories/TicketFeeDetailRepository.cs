using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITicketFeeDetailRepository : IOrmRepository<TicketFeeDetail, TicketFeeDetail>
    {
        TicketFeeDetail Get(int id);

        IEnumerable<TicketFeeDetail> GetByEventTicketAttributeIds(IEnumerable<long> ids);

        TicketFeeDetail GetByEventTicketAttributeId(long id);

        TicketFeeDetail GetByEventTicketAttributeIdAndFeedId(long EventTicketAttributeId, short FeedId);

        IEnumerable<TicketFeeDetail> GetAllByEventTicketAttributeId(long id);

        IEnumerable<TicketFeeDetail> GetAllEnabledByEventTicketAttributeIds(IEnumerable<long> ids);
    }

    public class TicketFeeDetailRepository : BaseOrmRepository<TicketFeeDetail>, ITicketFeeDetailRepository
    {
        public TicketFeeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketFeeDetail Get(int id)
        {
            return Get(new TicketFeeDetail { Id = id });
        }

        public IEnumerable<TicketFeeDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteTicketFeeDetail(TicketFeeDetail ticketFeeDetail)
        {
            Delete(ticketFeeDetail);
        }

        public TicketFeeDetail SaveTicketFeeDetail(TicketFeeDetail ticketFeeDetail)
        {
            return Save(ticketFeeDetail);
        }

        public IEnumerable<TicketFeeDetail> GetByEventTicketAttributeIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(TicketFeeDetail.EventTicketAttributeId):C} IN @EventTicketAttributeId")
                        .WithParameters(new { EventTicketAttributeId = ids }));
        }

        public TicketFeeDetail GetByEventTicketAttributeId(long id)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(TicketFeeDetail.EventTicketAttributeId):C} = @EventTicketAttributeId")
                        .WithParameters(new { EventTicketAttributeId = id })).FirstOrDefault();
        }

        public IEnumerable<TicketFeeDetail> GetAllByEventTicketAttributeId(long id)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(TicketFeeDetail.EventTicketAttributeId):C} = @EventTicketAttributeId")
                        .WithParameters(new { EventTicketAttributeId = id }));
        }

        public TicketFeeDetail GetByEventTicketAttributeIdAndFeedId(long EventTicketAttributeId, short FeedId)
        {
            return GetAll(s => s.Where($"{nameof(TicketFeeDetail.EventTicketAttributeId):C} = @EventTicketAttributeId AND  {nameof(TicketFeeDetail.FeeId):C} = @FeedId")
                .WithParameters(new { EventTicketAttributeId = EventTicketAttributeId, FeedId = FeedId })
            ).FirstOrDefault();
        }

        public IEnumerable<TicketFeeDetail> GetAllEnabledByEventTicketAttributeIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(TicketFeeDetail.EventTicketAttributeId):C} IN @EventTicketAttributeId AND IsEnabled=1")
                        .WithParameters(new { EventTicketAttributeId = ids }));
        }
    }
}