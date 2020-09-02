using FIL.Api.Repositories;
using FIL.Contracts.Queries.ProductEvent;
using FIL.Contracts.QueryResults.ProductEvent;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.ProductEvent
{
    public class ProductEventQueryHandler : IQueryHandler<ProductEventQuery, ProductEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;

        public ProductEventQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public ProductEventQueryResult Handle(ProductEventQuery query)
        {
            var eventResult = _eventRepository.GetEventsByProduct(query.IsFeel);
            var productEvents = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(eventResult);
            return new ProductEventQueryResult
            {
                Events = productEvents,
            };
        }
    }
}