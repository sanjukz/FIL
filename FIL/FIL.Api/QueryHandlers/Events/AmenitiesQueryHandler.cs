using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Api.QueryHandlers.Events
{
    public class AmenitiesQueryHandler : IQueryHandler<AmenitiesQuery, AmenitiesQueryResult>
    {
        private readonly IAmenityRepository _amenityRepository;

        public AmenitiesQueryHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository;
        }

        public AmenitiesQueryResult Handle(AmenitiesQuery query)
        {
            var eventResult = _amenityRepository.GetAll();
            return new AmenitiesQueryResult
            {
                Amenities = AutoMapper.Mapper.Map<System.Collections.Generic.List<FIL.Contracts.Models.Amenities>>(eventResult)
            };
        }
    }
}