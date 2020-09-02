using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS
{
    public class CartDataQueryHandler : IQueryHandler<CartDataQuery, CartDataQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IFeaturedEventRepository _featuredEventRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;

        public CartDataQueryHandler(IEventDetailRepository eventDetailRepository, IFeaturedEventRepository featuredEventRepository, IEventTicketDetailRepository eventTicketDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository, IVenueRepository venueRepository, ICityRepository cityRepository, ITicketCategoryRepository ticketCategoryRepository, ICurrencyTypeRepository currencyTypeRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _featuredEventRepository = featuredEventRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
        }

        public CartDataQueryResult Handle(CartDataQuery query)
        {
            List<CartDataModel> cartDataModelList = new List<CartDataModel>();

            foreach (var eventDetail in query.EventDetailids)
            {
                CartDataModel cartDataModel = new CartDataModel();
                var eventDetailDataModel = _eventDetailRepository.GetById(eventDetail);
                var venueDataModal = _venueRepository.GetByVenueId(eventDetailDataModel.VenueId);
                var cityDataModal = _cityRepository.GetByCityId(venueDataModal.CityId);
                var eventTicketDetailDataModel = _eventTicketDetailRepository.GetAllByTicketCategoryIdAndEventDetailId(query.TicketCategoryId, eventDetail);
                var tickcategoryDataModal = _ticketCategoryRepository.Get(query.TicketCategoryId);
                var eventTicketAttributeDataModel = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetailDataModel.Id);
                var eventDeliveryTypeDataModel = _eventDeliveryTypeDetailRepository.GetByEventDetailId(eventDetail);
                var currencyTypeDataModel = _currencyTypeRepository.GetById(eventTicketAttributeDataModel.LocalCurrencyId);
                cartDataModel.Name = eventDetailDataModel.Name;
                cartDataModel.startDateTime = eventDetailDataModel.StartDateTime.ToString();
                cartDataModel.venueName = venueDataModal.Name;
                cartDataModel.city = cityDataModal.Name;
                cartDataModel.ticketCategoryName = tickcategoryDataModal.Name;
                cartDataModel.price = eventTicketAttributeDataModel.LocalPrice;
                cartDataModel.deliveryType = DeliveryTypes.PrintAtHome.ToString();
                cartDataModel.currencyCode = currencyTypeDataModel.Code;
                cartDataModelList.Add(cartDataModel);
            }

            return new CartDataQueryResult
            {
                cartDataList = cartDataModelList
            };
        }
    }
}