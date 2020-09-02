using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayoutSections;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;

namespace FIL.Api.QueryHandlers.MasterVenueLayoutSections
{
    public class SectionDetailsByVenueLayoutQueryHandler : IQueryHandler<SectionDetailsByVenueLayoutQuery, SectionDetailsByVenueLayoutQueryResult>
    {
        private IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private IMasterVenueLayoutRepository _masterVenueLayoutRepository;

        public SectionDetailsByVenueLayoutQueryHandler(IMasterVenueLayoutRepository masterVenueLayoutRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
        }

        public SectionDetailsByVenueLayoutQueryResult Handle(SectionDetailsByVenueLayoutQuery query)
        {
            var masterVenueLayout = _masterVenueLayoutRepository.GetByAltId(query.AltId);
            var sectionDetailsByVenueLayout = _masterVenueLayoutSectionRepository.SectionDetailsByVenueLayout(masterVenueLayout.Id);
            return new SectionDetailsByVenueLayoutQueryResult { SectionDetailsByVenueLayout = sectionDetailsByVenueLayout };
        }
    }
}