using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICorporateTransactionDetailRepository : IOrmRepository<CorporateTransactionDetail, CorporateTransactionDetail>
    {
        CorporateTransactionDetail Get(long id);

        IEnumerable<CorporateTransactionDetail> GetByFailedEventTicketAttributeId(long eventTicketAttributeId);

        IEnumerable<CorporateTransactionDetail> GetBySuccessfulCompsEventTicketAttributeId(long eventTicketAttributeId);

        IEnumerable<CorporateTransactionDetail> GetBySuccessfulPaidEventTicketAttributeId(long eventTicketAttributeId);

        IEnumerable<CorporateTransactionDetail> GetByEventTicketAttributeIds(IEnumerable<long> eventTicketAttributeIds);
    }

    public class CorporateTransactionDetailRepository : BaseLongOrmRepository<CorporateTransactionDetail>, ICorporateTransactionDetailRepository
    {
        public CorporateTransactionDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateTransactionDetail Get(long id)
        {
            return Get(new CorporateTransactionDetail { Id = id });
        }

        public IEnumerable<CorporateTransactionDetail> GetByFailedEventTicketAttributeId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTransactionDetail.EventTicketAttributeId):c}=@EventTicketAttributeId And IsEnabled=0")
            .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId }));
        }

        public IEnumerable<CorporateTransactionDetail> GetBySuccessfulCompsEventTicketAttributeId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTransactionDetail.EventTicketAttributeId):c}=@EventTicketAttributeId And IsEnabled=1 And TransactingOptionId=2")
            .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId }));
        }

        public IEnumerable<CorporateTransactionDetail> GetBySuccessfulPaidEventTicketAttributeId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTransactionDetail.EventTicketAttributeId):c}=@EventTicketAttributeId And IsEnabled=1 And TransactingOptionId=1")
            .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId }));
        }

        public IEnumerable<CorporateTransactionDetail> GetByEventTicketAttributeIds(IEnumerable<long> eventTicketAttributeIds)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTransactionDetail.EventTicketAttributeId):c} IN @EventTicketAttributeIds")
            .WithParameters(new { EventTicketAttributeIds = eventTicketAttributeIds }));
        }
    }
}