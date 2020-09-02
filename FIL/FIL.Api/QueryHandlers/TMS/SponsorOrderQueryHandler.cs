using FIL.Api.Repositories;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class SponsorOrderQueryHandler : IQueryHandler<SponsorOrderQuery, SponsorOrderQueryResult>
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;

        public SponsorOrderQueryHandler(
           ISponsorRepository sponsorRepository,
            ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository)
        {
            _sponsorRepository = sponsorRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
        }

        public SponsorOrderQueryResult Handle(SponsorOrderQuery query)
        {
            var corporateTicketAllocationDetails = _corporateTicketAllocationDetailRepository.GetByEventTicketAttributeId(query.EventTicketAttributeId);
            var sponsors = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Sponsor>>(_sponsorRepository.GetByIds(corporateTicketAllocationDetails.Select(s => s.SponsorId).Distinct()).ToList());
            return new SponsorOrderQueryResult
            {
                sponsors = sponsors
            };
        }
    }
}