using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.BoxOffice;
using FIL.Contracts.Queries.BoxOffice.TicketCategory;
using FIL.Contracts.QueryResults.BoxOffice.TicketCategories;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice.TicketCategory
{
    public class SeasonTicketCategoryQueryHandler : IQueryHandler<SeasonTicketCategoryQuery, SeasonTicketCategoryQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public SeasonTicketCategoryQueryHandler(
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository
         )
        {
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public SeasonTicketCategoryQueryResult Handle(SeasonTicketCategoryQuery query)
        {
            var eventId = _eventDetailRepository.GetByEventAltId(query.EventDetailAltId).EventId;
            var venueId = _venueRepository.GetByAltId(query.VenueAltId).Id;

            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueId(eventId, venueId).Distinct().ToDictionary(ed => ed.Id);
            var eventTicketDetailDataModel = _eventTicketDetailRepository.GetByBOTicketCategoryIdAndeventDetailIds(query.TicketCategoryId, eventDetails.Values.Select(ed => ed.Id)).ToDictionary(etd => etd.Id);
            var eventTicketCategoryDataModel = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailDataModel.Values.Select(etd => etd.TicketCategoryId).Distinct()).ToDictionary(tc => tc.Id);
            var eventTicketAttributeModel = _eventTicketAttributeRepository.GetSeasonByEventTicketDetailId(eventTicketDetailDataModel.Values.Select(eta => eta.Id)).Distinct().ToDictionary(eta => eta.Id);

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

            var ticketCategoryContainer = eventTicketAttributeModel.Values.Select(etAttribute =>
            {
                var eventTicketDetail = eventTicketDetailDataModel[etAttribute.EventTicketDetailId];
                var eventDetail = eventDetails[eventTicketDetail.EventDetailId];
                var ticketCategory = eventTicketCategoryDataModel[(int)eventTicketDetail.TicketCategoryId];
                var eventTicketAttribute = eventTicketAttributeModel[etAttribute.Id];
                var currencyType = GetCurrencyType(eventTicketAttribute.LocalCurrencyId); ;

                return new TicketCategoryContainer
                {
                    TicketCategory = AutoMapper.Mapper.Map<Contracts.Models.TicketCategory>(ticketCategory),
                    EventTicketDetail = AutoMapper.Mapper.Map<Contracts.Models.EventTicketDetail>(eventTicketDetail),
                    EventDetail = AutoMapper.Mapper.Map<Contracts.Models.EventDetail>(eventDetail),
                    EventTicketAttribute = AutoMapper.Mapper.Map<Contracts.Models.EventTicketAttribute>(eventTicketAttribute),
                    CurrencyType = AutoMapper.Mapper.Map<Contracts.Models.CurrencyType>(currencyType),
                };
            });

            return new SeasonTicketCategoryQueryResult
            {
                ticketCategoryContainer = ticketCategoryContainer.ToList(),
            };
        }
    }
}