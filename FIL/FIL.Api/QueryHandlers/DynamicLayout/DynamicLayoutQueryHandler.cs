using FIL.Api.Repositories;
using FIL.Contracts.Queries.DynamicLayout;
using FIL.Contracts.QueryResults.DynamicLayout;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.DynamicLayout
{
    public class DynamicLayoutQueryHandler : IQueryHandler<DynamicLayoutQuery, DynamicLayoutQueryResult>
    {
        private readonly IDynamicStadiumCoordinateRepository _dynamicStadiumCoordinateRepository;
        private readonly IDynamicStadiumTicketCategoriesDetailsRepository _dynamicStadiumTicketCategoriesDetailsRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IPathTypeRepository _pathTypeRepository;

        public DynamicLayoutQueryHandler(IDynamicStadiumCoordinateRepository dynamicStadiumCoordinateRepository,
            IDynamicStadiumTicketCategoriesDetailsRepository dynamicStadiumTicketCategoriesDetailsRepository,
            IPathTypeRepository pathTypeRepository,
            IVenueRepository venueRepository)
        {
            _dynamicStadiumCoordinateRepository = dynamicStadiumCoordinateRepository;
            _dynamicStadiumTicketCategoriesDetailsRepository = dynamicStadiumTicketCategoriesDetailsRepository;
            _venueRepository = venueRepository;
            _pathTypeRepository = pathTypeRepository;
        }

        public DynamicLayoutQueryResult Handle(DynamicLayoutQuery query)
        {
            if (query.VenueId == 0 && query.VenueAltId == null)
            {
                return new DynamicLayoutQueryResult { };
            }

            if (query.VenueId == 0)
            {
                query.VenueId = _venueRepository.GetByAltId(query.VenueAltId).Id;
            }
            var DynamicStadiumCoordinateDataModel = _dynamicStadiumCoordinateRepository.GetByVenueId(query.VenueId);
            var DynamicStadiumCoordinateModel = AutoMapper.Mapper.Map<List<Contracts.Models.DynamicStadiumCoordinate>>(DynamicStadiumCoordinateDataModel);

            var DynamicStadiumTicketCategoriesDetailDataModel = _dynamicStadiumTicketCategoriesDetailsRepository.GetByDynamicStadiumCoordinateId(DynamicStadiumCoordinateModel.Select(s => s.Id));
            var DynamicStadiumTicketCategoriesDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.DynamicStadiumTicketCategoriesDetails>>(DynamicStadiumTicketCategoriesDetailDataModel);

            var pathTypes = _pathTypeRepository.GetAll().ToList();

            return new DynamicLayoutQueryResult
            {
                DynamicStadiumCoordinate = DynamicStadiumCoordinateModel,
                PathTypes = pathTypes,
                DynamicStadiumTicketCategoriesDetails = DynamicStadiumTicketCategoriesDetailModel
            };
        }
    }
}