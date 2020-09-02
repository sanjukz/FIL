using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.BoxOffice;
using FIL.Contracts.Queries.BoxOffice.TicketCategory;
using FIL.Contracts.QueryResults.Boxoffice.TicketCategories;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice.TicketCategory
{
    public class TicketCategoryQueryHandler : IQueryHandler<TicketCategoryQuery, TicketCategoryQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IZsuiteUserFeeDetailRepository _zsuiteUserFeeDetailRepository;
        private readonly IZsuitePaymentOptionRepository _zsuitePaymentOptionRepository;

        public TicketCategoryQueryHandler(
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IUserRepository userRepository,
            IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository,
            ICityRepository cityRepository,
            IZsuiteUserFeeDetailRepository zsuiteUserFeeDetailRepository,
            IZsuitePaymentOptionRepository zsuitePaymentOptionRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _userRepository = userRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _cityRepository = cityRepository;
            _zsuiteUserFeeDetailRepository = zsuiteUserFeeDetailRepository;
            _zsuitePaymentOptionRepository = zsuitePaymentOptionRepository;
        }

        public TicketCategoryQueryResult Handle(TicketCategoryQuery query)
        {
            var userId = _userRepository.GetByAltId(query.UserAltId).Id;
            var boxofficeUserAdditionalDetailModel = AutoMapper.Mapper.Map<Contracts.Models.BoxofficeUserAdditionalDetail>(_boxofficeUserAdditionalDetailRepository.GetByUserId(userId));
            var zsuiteUserFeeDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.ZsuiteUserFeeDetail>>(_zsuiteUserFeeDetailRepository.GetByBoxOfficeUserAdditionalDetailId(boxofficeUserAdditionalDetailModel.Id));
            var eventDetailDataModel = _eventDetailRepository.GetByEventAltId(query.EventDetailAltId);
            var eventDetailsDataModel = AutoMapper.Mapper.Map<Contracts.Models.EventDetail>(eventDetailDataModel);
            var venue = _venueRepository.Get(eventDetailDataModel.VenueId);
            var venueModel = AutoMapper.Mapper.Map<Contracts.Models.Venue>(venue);
            var city = _cityRepository.Get(venueModel.CityId);
            var cityModel = AutoMapper.Mapper.Map<Contracts.Models.City>(city);
            var eventTicketDetailDataModel = _eventTicketDetailRepository.GetBOByEventDetailId(eventDetailDataModel.Id);
            var eventTicketAttributeDataModel = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetailDataModel.Select(etd => etd.Id).Distinct()).ToDictionary(eta => eta.EventTicketDetailId);
            var eventTicketCategoryDataModel = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailDataModel.Select(etd => etd.TicketCategoryId).Distinct()).ToDictionary(tc => tc.Id);
            var zsuitepaymetOptionModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.ZsuitePaymentOption>>(_zsuitePaymentOptionRepository.GetAll());

            var currencyTypeCache = new Dictionary<int, CurrencyType>();
            CurrencyType GetCurrencyType(int localCurrencyId)
            {
                if (currencyTypeCache.ContainsKey(localCurrencyId))
                {
                    return currencyTypeCache[localCurrencyId];
                }
                var currencyType = _currencyTypeRepository.Get(localCurrencyId);
                currencyTypeCache.Add(localCurrencyId, currencyType);
                return currencyType;
            }

            var ticketCategoryContainer = eventTicketDetailDataModel.Select(etdetail =>
            {
                var ticketCategory = eventTicketCategoryDataModel[(int)etdetail.TicketCategoryId];
                var eventTicketAttribute = eventTicketAttributeDataModel[etdetail.Id];
                var currencyType = GetCurrencyType(eventTicketAttribute.LocalCurrencyId);

                return new TicketCategoryContainer
                {
                    TicketCategory = AutoMapper.Mapper.Map<Contracts.Models.TicketCategory>(ticketCategory),
                    EventTicketAttribute = AutoMapper.Mapper.Map<Contracts.Models.EventTicketAttribute>(eventTicketAttribute),
                    CurrencyType = AutoMapper.Mapper.Map<Contracts.Models.CurrencyType>(currencyType),
                };
            });

            return new TicketCategoryQueryResult
            {
                eventDetail = eventDetailsDataModel,
                venue = venueModel,
                city = cityModel,
                ticketCategoryContainer = ticketCategoryContainer.ToList(),
                boxofficeUserAdditionalDetail = boxofficeUserAdditionalDetailModel,
                zsuiteUserFeeDetail = zsuiteUserFeeDetailModel,
                zsuitePaymentOptions = zsuitepaymetOptionModel
            };
        }
    }
}