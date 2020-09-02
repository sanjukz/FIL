using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.TMS;
using FIL.Contracts.Queries.TMS.Booking;
using FIL.Contracts.QueryResults.TMS.Booking;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS.Booking
{
    public class CorporateOrderQueryHandler : IQueryHandler<CorporateOrderQuery, CorporateOrderQueryResult>
    {
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;

        public CorporateOrderQueryHandler(ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository)
        {
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
        }

        public CorporateOrderQueryResult Handle(CorporateOrderQuery query)
        {
            List<CorporateTicketAllocationDetail> corporateTicketAllocationDetails = new List<CorporateTicketAllocationDetail>();
            if (query.AllocationTypeId == AllocationType.Match)
            {
                var corporateTicketAllocationId = query.corpoarteOrderDetails.Select(s => (long)s.CorporateTicketAllocationId).Distinct().First();
                corporateTicketAllocationDetails = AutoMapper.Mapper.Map<List<CorporateTicketAllocationDetail>>(_corporateTicketAllocationDetailRepository.GetCorporateDetails(corporateTicketAllocationId));
            }
            if (query.AllocationTypeId == AllocationType.Venue)
            {
                var sponsorId = query.corpoarteOrderDetails.Select(s => (long)s.SponsorId).Distinct().First();
                var ticketCategoryId = query.corpoarteOrderDetails.Select(s => (long)s.TicketCategoryId).Distinct().First();
                var eventDetailIds = query.corpoarteOrderDetails.Select(s => (long)s.EventDetailId).Distinct();
                corporateTicketAllocationDetails = AutoMapper.Mapper.Map<List<CorporateTicketAllocationDetail>>(_corporateTicketAllocationDetailRepository.GetCorporateDetails(sponsorId, ticketCategoryId, query.corpoarteOrderDetails.Select(s => (long)s.EventDetailId).Distinct()));
            }
            return new CorporateOrderQueryResult
            {
                corporateTicketAllocationDetails = corporateTicketAllocationDetails
            };
        }
    }
}