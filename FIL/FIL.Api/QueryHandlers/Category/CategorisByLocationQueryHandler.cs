using FIL.Api.Repositories;
using FIL.Contracts.Queries.Category;
using FIL.Contracts.QueryResults.Category;
using System.Linq;

namespace FIL.Api.QueryHandlers.Category
{
    public class CategorisByLocationQueryHandler : IQueryHandler<CategoriesByLocationQuery, CategoriesByLocationQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public CategorisByLocationQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
        }

        public CategoriesByLocationQueryResult Handle(CategoriesByLocationQuery query)
        {
            var eventCategories = _eventCategoryRepository.GetCategoriesByLocation(query.CityIds).ToList();
            return new CategoriesByLocationQueryResult
            {
                EventCategories = eventCategories
            };
        }
    }
}