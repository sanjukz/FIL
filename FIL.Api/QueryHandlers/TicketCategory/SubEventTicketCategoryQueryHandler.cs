using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketCategory;
using FIL.Contracts.QueryResults.TicketCategories;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketCategory
{
    public class SubEventTicketCategoryQueryHandler : IQueryHandler<SubEventTicketCategoryQuery, SubEventTicketCategoryQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public SubEventTicketCategoryQueryHandler(IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository)

        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public SubEventTicketCategoryQueryResult Handle(SubEventTicketCategoryQuery query)
        {
            var eventDetailModelDataModel = _eventDetailRepository.GetById(query.EventDetailId);
            var eventDetailModel = AutoMapper.Mapper.Map<Contracts.Models.EventDetail>(eventDetailModelDataModel);

            var venueDetailDataModel = _venueRepository.Get(eventDetailModelDataModel.VenueId);
            var venueDetailModel = AutoMapper.Mapper.Map<Contracts.Models.Venue>(venueDetailDataModel);

            var cityDetailDataModel = _cityRepository.Get(venueDetailDataModel.CityId);
            var cityModel = AutoMapper.Mapper.Map<Contracts.Models.City>(cityDetailDataModel);

            var eventTicketDetailDataModel = _eventTicketDetailRepository.GetByEventDetailId(eventDetailModelDataModel.Id);
            var eventTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventTicketDetailDataModel);

            var ticketCategoryIdList = eventTicketDetailModel.Select(s => s.TicketCategoryId);
            var ticketCategoryDataModel = _ticketCategoryRepository.GetByEventDetailIds(ticketCategoryIdList);
            var ticketCategoryModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(ticketCategoryDataModel);

            var eventTicketDetailIdList = eventTicketDetailModel.Select(s => s.Id);
            var eventTicketDetailIdDataModel = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetailIdList);
            var eventTicketAttributeModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketDetailIdDataModel);

            var currencyList = eventTicketAttributeModel.Select(s => s.CurrencyId).Distinct().FirstOrDefault();
            var currencyModel = AutoMapper.Mapper.Map<Contracts.Models.CurrencyType>(_currencyTypeRepository.Get(currencyList));

            return new SubEventTicketCategoryQueryResult
            {
                EventDetail = eventDetailModel,
                EventTicketAttribute = eventTicketAttributeModel,
                Venue = venueDetailModel,
                City = cityModel,
                EventTicketDetail = eventTicketDetailModel,
                TicketCategory = ticketCategoryModel,
                CurrencyType = currencyModel
            };
        }
    }
}