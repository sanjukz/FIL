using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.SponsersSeat;
using FIL.Contracts.QueryResults.SponsersSeat;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketCategory
{
    public class SponsersSeatQueryHandler : IQueryHandler<SponsersSeatQuery, SponsersSeatQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IRASVTicketTypeMappingRepository _rasvTicketTypeMappingRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetail;
        private readonly ITicketFeeDetailRepository _ticketFeeDetail;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IMatchAttributeRepository _matchAttributeRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IPasswordHasher<string> _passwordHasher;

        public SponsersSeatQueryHandler(IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ISponsorRepository sponsorRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            ICityRepository cityRepository,
            IPasswordHasher<string> passwordHasher,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetail,
            IRASVTicketTypeMappingRepository rasvTicketTypeMappingRepository,
            ITicketFeeDetailRepository ticketFeeDetail,
            ITeamRepository teamRepository,
            IMatchAttributeRepository matchAttributeRepository)

        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _teamRepository = teamRepository;
            _passwordHasher = passwordHasher;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _matchAttributeRepository = matchAttributeRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _sponsorRepository = sponsorRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _rasvTicketTypeMappingRepository = rasvTicketTypeMappingRepository;
            _eventDeliveryTypeDetail = eventDeliveryTypeDetail;
            _ticketFeeDetail = ticketFeeDetail;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
        }

        public SponsersSeatQueryResult Handle(SponsersSeatQuery query)
        {
            var sponser = _sponsorRepository.GetByUserName(query.Email);

            var firstSponsor = sponser.FirstOrDefault();

            if (sponser != null)
            {
                var isAuthenticatedSponser = _passwordHasher.VerifyHashedPassword(query.Email, firstSponsor.Password, query.Password);
                if (isAuthenticatedSponser == PasswordVerificationResult.Success)
                {
                    List<Contracts.Models.MatchLayoutSectionSeat> MatchLayoutSectionSeatList = new List<Contracts.Models.MatchLayoutSectionSeat>();

                    List<Contracts.Models.MatchSeatTicketDetail> MatchSeatTicketDetailList = new List<Contracts.Models.MatchSeatTicketDetail>();

                    List<Contracts.Models.EventTicketDetail> sponserAssociatedEventTicketDetailList = new List<Contracts.Models.EventTicketDetail>();

                    var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetBySponserIds(sponser.Select(s => s.Id).ToList());

                    for (int k = 0; k < matchSeatTicketDetails.ToList().Count(); k = k + 2000)
                    {
                        var matchSeatTicketDetailListBatcher = matchSeatTicketDetails.Skip(k).Take(2000).ToList();

                        var matchSeatTicketDetailsModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.MatchSeatTicketDetail>>(matchSeatTicketDetailListBatcher);

                        var sponserAssociatedEventTicketDetails = _eventTicketDetailRepository.GetAllByEventTicketDetailIds(matchSeatTicketDetailsModel.Select(s => s.EventTicketDetailId).Distinct());

                        var sponserAssociatedEventTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(sponserAssociatedEventTicketDetails);

                        var MatchLayoutSectionSeatIds = matchSeatTicketDetailListBatcher.Select(s => s.MatchLayoutSectionSeatId);

                        List<Contracts.DataModels.MatchLayoutSectionSeat> matchLayoutSectionSeat = AutoMapper.Mapper.Map<List<Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByIds((IEnumerable<long>)MatchLayoutSectionSeatIds));

                        List<Contracts.Models.MatchLayoutSectionSeat> MatchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.Models.MatchLayoutSectionSeat>>(matchLayoutSectionSeat);

                        foreach (var item in MatchLayoutSectionSeats)
                        {
                            MatchLayoutSectionSeatList.Add(item);
                        }

                        foreach (var item in matchSeatTicketDetailsModel)
                        {
                            MatchSeatTicketDetailList.Add(item);
                        }
                        foreach (var item in sponserAssociatedEventTicketDetailModel)
                        {
                            sponserAssociatedEventTicketDetailList.Add(item);
                        }
                    }

                    var sponserAssociatedEventDetails = _eventDetailRepository.GetByEventDetailIds(sponserAssociatedEventTicketDetailList.Select(s => s.EventDetailId).Distinct()).ToList();

                    var sponserAssociatedTicketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(sponserAssociatedEventTicketDetailList.Select(s => s.TicketCategoryId).Distinct()).ToList();

                    var sponserAssociatedTicketCategoriesModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(sponserAssociatedTicketCategories);

                    if (sponserAssociatedEventDetails.Count() > 0)
                    {
                        var eventDataModel = _eventRepository.Get(sponserAssociatedEventDetails[0].EventId);

                        var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);

                        var eventDetailModelDataModel = _eventDetailRepository.GetByEventDetailIds(sponserAssociatedEventDetails.Select(s => s.Id).Distinct());

                        var eventDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDetail>>(eventDetailModelDataModel);

                        var eventDeliveryTypeDetailDataModel = _eventDeliveryTypeDetail.GetByEventDetailIds(eventDetailModelDataModel.Select(ed => ed.Id));
                        var eventDeliveryTypeDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDeliveryTypeDetail>>(eventDeliveryTypeDetailDataModel);

                        var RASVTicketTypeMappingsDataModel = _rasvTicketTypeMappingRepository.GetByEventDetailIds(eventDetailModelDataModel.Select(ed => ed.Id)).Where(sed => sed.IsEnabled == true);
                        var RASVTicketTypeMappingsModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.RASVTicketTypeMapping>>(RASVTicketTypeMappingsDataModel);

                        var eventVenueList = eventDetailModelDataModel.Select(s => s.VenueId);
                        var venueDetailDataModel = _venueRepository.GetByVenueIds(eventVenueList);
                        var venueDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Venue>>(venueDetailDataModel);

                        var CityList = venueDetailDataModel.Select(s => s.CityId);
                        var cityDetailDataModel = _cityRepository.GetByCityIds(CityList);
                        var cityModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.City>>(cityDetailDataModel);

                        var eventTicketDetailList = eventDetailModelDataModel.Select(s => s.Id);
                        var eventTicketDetailDataModel = _eventTicketDetailRepository.GetAllByEventDetailIds(eventTicketDetailList);
                        var eventTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventTicketDetailDataModel);

                        var matchAttribute = _matchAttributeRepository.GetByEventDetailIds(eventDetailModelDataModel.Select(ed => ed.Id).Distinct());
                        var team = _teamRepository.GetAll();

                        if (eventTicketDetailModel != null)
                        {
                            var ticketCategoryIdList = eventTicketDetailModel.Select(s => s.TicketCategoryId);
                            var ticketCategoryDataModel = _ticketCategoryRepository.GetAllTicketCategory(ticketCategoryIdList);
                            var ticketCategoryModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(ticketCategoryDataModel);

                            var eventTicketDetailIdList = eventTicketDetailModel.Select(s => s.Id);

                            var partialSponsor = _sponsorRepository.Get(4066);

                            var killSponsor = _sponsorRepository.Get(949);

                            var partialMatchSeatTicketDetails = _matchSeatTicketDetailRepository.GetBySponsorIdAndEventTicketDetailIds(partialSponsor.Id, eventTicketDetailIdList);

                            var partialMatchSeatTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.MatchSeatTicketDetail>>(partialMatchSeatTicketDetails);

                            var killMatchSeatTicketDetails = _matchSeatTicketDetailRepository.GetBySponsorIdAndEventTicketDetailIds(killSponsor.Id, eventTicketDetailIdList);

                            var killMatchSeatTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.MatchSeatTicketDetail>>(killMatchSeatTicketDetails);

                            var eventTicketDetailIdDataModel = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetailIdList);

                            var eventTicketAttributeModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketDetailIdDataModel);

                            var eventTicketAttributeIdList = eventTicketAttributeModel.Select(s => s.Id);
                            var ticketFeeDetailIdDataModel = _ticketFeeDetail.GetByEventTicketAttributeIds(eventTicketAttributeIdList);
                            var ticketFeeDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketFeeDetail>>(ticketFeeDetailIdDataModel);

                            var currencyList = eventTicketAttributeModel.Select(s => s.CurrencyId).Distinct().FirstOrDefault();
                            var currencyModel = AutoMapper.Mapper.Map<Contracts.Models.CurrencyType>(_currencyTypeRepository.Get(currencyList));

                            return new SponsersSeatQueryResult
                            {
                                IsauthenticatedSponser = true,
                                Event = eventModel,
                                EventDetail = eventDetailModel,
                                EventTicketAttribute = eventTicketAttributeModel,
                                TicketFeeDetail = ticketFeeDetailModel,
                                Venue = venueDetailModel,
                                City = cityModel,
                                EventTicketDetail = eventTicketDetailModel,
                                TicketCategory = ticketCategoryModel,
                                CurrencyType = currencyModel,
                                RASVTicketTypeMappings = RASVTicketTypeMappingsModel,
                                EventDeliveryTypeDetails = eventDeliveryTypeDetailModel,
                                EventCategory = eventModel.EventCategoryId,
                                MatchAttribute = Mapper.Map<IEnumerable<MatchAttribute>>(matchAttribute),
                                Team = Mapper.Map<IEnumerable<Team>>(team),
                                matchLayoutSectionSeats = MatchLayoutSectionSeatList,
                                matchSeatTicketDetails = MatchSeatTicketDetailList,
                                SponserAssociatedTicketCategory = sponserAssociatedTicketCategoriesModel,
                                SponserAssociatedEventTicketDetails = sponserAssociatedEventTicketDetailList,
                                PartialViewSponsors = partialMatchSeatTicketDetailModel,
                                KillViewSponsors = killMatchSeatTicketDetailModel
                            };
                        }
                        else
                        {
                            return new SponsersSeatQueryResult
                            {
                                IsauthenticatedSponser = true,
                                Event = eventModel,
                                EventDetail = eventDetailModel,
                                EventTicketAttribute = null,
                                TicketFeeDetail = null,
                                Venue = venueDetailModel,
                                City = cityModel,
                                EventTicketDetail = eventTicketDetailModel,
                                TicketCategory = null,
                                CurrencyType = null,
                                RASVTicketTypeMappings = RASVTicketTypeMappingsModel,
                                EventDeliveryTypeDetails = eventDeliveryTypeDetailModel,
                                EventCategory = eventModel.EventCategoryId,
                                MatchAttribute = null,
                                Team = null,
                                matchLayoutSectionSeats = null,
                                matchSeatTicketDetails = null,
                                SponserAssociatedTicketCategory = null,
                                SponserAssociatedEventTicketDetails = null,
                                PartialViewSponsors = null,
                                KillViewSponsors = null
                            };
                        }
                    }
                    else
                    {
                        return new SponsersSeatQueryResult
                        {
                            IsauthenticatedSponser = true,
                            Event = null,
                            EventDetail = null,
                            EventTicketAttribute = null,
                            TicketFeeDetail = null,
                            Venue = null,
                            City = null,
                            EventTicketDetail = null,
                            TicketCategory = null,
                            CurrencyType = null,
                            RASVTicketTypeMappings = null,
                            EventDeliveryTypeDetails = null,
                            EventCategory = 0,
                            MatchAttribute = null,
                            Team = null,
                            SponserAssociatedTicketCategory = null,
                            matchLayoutSectionSeats = null,
                            matchSeatTicketDetails = null,
                            SponserAssociatedEventTicketDetails = null,
                            PartialViewSponsors = null,
                            KillViewSponsors = null
                        };
                    }
                }
                else
                {
                    return new SponsersSeatQueryResult
                    {
                        IsauthenticatedSponser = false,
                        Event = null,
                        EventDetail = null,
                        EventTicketAttribute = null,
                        TicketFeeDetail = null,
                        Venue = null,
                        City = null,
                        EventTicketDetail = null,
                        TicketCategory = null,
                        CurrencyType = null,
                        RASVTicketTypeMappings = null,
                        EventDeliveryTypeDetails = null,
                        EventCategory = 0,
                        MatchAttribute = null,
                        SponserAssociatedTicketCategory = null,
                        Team = null,
                        matchLayoutSectionSeats = null,
                        matchSeatTicketDetails = null,
                        SponserAssociatedEventTicketDetails = null,
                        PartialViewSponsors = null,
                        KillViewSponsors = null
                    };
                }
            }
            else
            {
                return new SponsersSeatQueryResult
                {
                    IsauthenticatedSponser = false,
                    Event = null,
                    EventDetail = null,
                    EventTicketAttribute = null,
                    TicketFeeDetail = null,
                    Venue = null,
                    City = null,
                    EventTicketDetail = null,
                    TicketCategory = null,
                    CurrencyType = null,
                    RASVTicketTypeMappings = null,
                    EventDeliveryTypeDetails = null,
                    SponserAssociatedTicketCategory = null,
                    EventCategory = 0,
                    MatchAttribute = null,
                    Team = null,
                    matchLayoutSectionSeats = null,
                    matchSeatTicketDetails = null,
                    SponserAssociatedEventTicketDetails = null,
                    PartialViewSponsors = null,
                    KillViewSponsors = null
                };
            }
        }
    }
}