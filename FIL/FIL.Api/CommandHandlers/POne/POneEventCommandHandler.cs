using FIL.Api.Integrations;
using FIL.Api.Integrations.POne;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.POne;
using FIL.Contracts.DataModels.POne;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.Integrations.POne;
using FIL.Contracts.Models.POne;
using FIL.Logging;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.POne
{
    public class POneEventCommandHandler : BaseCommandHandler<POneEventCommand>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IMediator _mediator;
        private readonly IPOneEventRepository _pOneEventRepository;
        private readonly IPOneEventDetailRepository _pOneEventDetailRepository;
        private readonly IPOneEventCategoryRepository _pOneEventCategoryRepository;
        private readonly IPOneEventSubCategoryRepository _pOneEventSubCategoryRepository;
        private readonly IPOneEventTicketAttributeRepository _pOneEventTicketAttributeRepository;
        private readonly IPOneEventTicketDetailRepository _pOneEventTicketDetailRepository;
        private readonly IPOneTicketCategoryRepository _pOneTicketCategoryRepository;
        private readonly IPOneVenueRepository _pOneVenueRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IPOneApi _pOneApi;
        private readonly IGoogleMapApi _googleMapApi;
        private readonly ICountryAlphaCode _countryAlphaCode;

        public POneEventCommandHandler(
            ILogger logger,
            ISettings settings,
            IMediator mediator,
            IPOneVenueRepository pOneVenueRepository,
            IPOneTicketCategoryRepository pOneTicketCategoryRepository,
            IPOneEventTicketDetailRepository pOneEventTicketDetailRepository,
            IPOneEventTicketAttributeRepository pOneEventTicketAttributeRepository,
            IPOneEventSubCategoryRepository pOneEventSubCategoryRepository,
            IPOneEventRepository pOneEventRepository,
            IPOneEventDetailRepository pOneEventDetailRepository,
            IPOneEventCategoryRepository pOneEventCategoryRepository,
            ICountryRepository countryRepository,
            IStateRepository stateRepository,
            ICityRepository cityRepository,
            IPOneApi pOneApi,
            IGoogleMapApi googleMapApi,
            ICountryAlphaCode countryAlphaCode
            )
           : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _mediator = mediator;
            _pOneEventCategoryRepository = pOneEventCategoryRepository;
            _pOneEventDetailRepository = pOneEventDetailRepository;
            _pOneEventRepository = pOneEventRepository;
            _pOneEventSubCategoryRepository = pOneEventSubCategoryRepository;
            _pOneEventTicketAttributeRepository = pOneEventTicketAttributeRepository;
            _pOneEventTicketDetailRepository = pOneEventTicketDetailRepository;
            _pOneTicketCategoryRepository = pOneTicketCategoryRepository;
            _pOneVenueRepository = pOneVenueRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _pOneApi = pOneApi;
            _googleMapApi = googleMapApi;
            _countryAlphaCode = countryAlphaCode;
        }

        protected override async Task Handle(POneEventCommand command)
        {
            var request = new POneApiRequestModel
            {
                language = "en"
            };
            var response = await _pOneApi.GetPOneApiData(request);
            var eventDictionary = JsonConvert.DeserializeObject<Dictionary<long, SkuModel>>(response.Result);

            eventDictionary = eventDictionary.Skip(command.Offset).Take(500).ToDictionary(s => s.Key, s => s.Value);

            Dictionary<long, SkuModel>.KeyCollection skuKeysColl = eventDictionary.Keys;

            foreach (var skuKey in skuKeysColl)
            {
                var pOneEvent = eventDictionary[2517000436527];
                if (pOneEvent.formatted_address == "-" || pOneEvent.formatted_address == "0" || pOneEvent.formatted_address == "unknown") continue;

                //saving event category
                var p1EventCategory = _pOneEventCategoryRepository.GetByName(pOneEvent.languages.EN.category);
                if (p1EventCategory == null)
                {
                    p1EventCategory = _pOneEventCategoryRepository.Save(new POneEventCategory
                    {
                        Name = pOneEvent.languages.EN.category,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow
                    });
                }

                var p1SubCategory = _pOneEventSubCategoryRepository.GetByNameAndPOneEventCategoryId(pOneEvent.languages.EN.sub_category, p1EventCategory.Id);
                if (p1SubCategory == null)
                {
                    p1SubCategory = _pOneEventSubCategoryRepository.Save(new POneEventSubCategory
                    {
                        Name = pOneEvent.languages.EN.sub_category,
                        POneEventCategoryId = p1EventCategory.Id,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }

                // saving p1 event
                var eventName = GetEventName(pOneEvent);
                var @event = _pOneEventRepository.GetByPOneId(Convert.ToInt32(pOneEvent.event_id));
                if (@event == null)
                {
                    @event = _pOneEventRepository.Save(new POneEvent
                    {
                        POneId = Convert.ToInt32(pOneEvent.event_id),
                        Name = eventName,
                        Description = string.Empty,
                        TermsAndConditions = string.Empty,
                        POneEventCategoryId = p1EventCategory.Id,
                        POneEventSubCategoryId = p1SubCategory.Id,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    @event.POneEventCategoryId = p1EventCategory.Id;
                    @event.POneEventSubCategoryId = p1SubCategory.Id;
                    @event.UpdatedUtc = DateTime.UtcNow;
                    @event = _pOneEventRepository.Save(@event);
                }

                // saving venue here
                var cityName = pOneEvent.languages.EN.city;
                var placeData = _googleMapApi.GetLatLongFromAddress(pOneEvent.formatted_address).Result.Result;

                if ((cityName == "unknown" || cityName == "-") && placeData != null)
                {
                    cityName = placeData.CityName;
                }

                var country = _countryRepository.GetByName(pOneEvent.languages.EN.country);
                var countryData = _countryAlphaCode.GetCountryCodeByName(pOneEvent.languages.EN.country).Result.Result;
                if (country == null)
                {
                    country = _countryRepository.Save(new Contracts.DataModels.Country
                    {
                        AltId = Guid.NewGuid(),
                        Name = pOneEvent.languages.EN.country,
                        IsoAlphaTwoCode = countryData.IsoAlphaTwoCode,
                        IsoAlphaThreeCode = countryData.IsoAlphaThreeCode,
                        CountryName = pOneEvent.languages.EN.country,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid(),
                        UpdatedBy = Guid.NewGuid()
                    });
                }
                else
                {
                    country.IsoAlphaTwoCode = countryData.IsoAlphaTwoCode;
                    country.IsoAlphaThreeCode = countryData.IsoAlphaThreeCode;
                    country.UpdatedUtc = DateTime.UtcNow;
                    _countryRepository.Save(country);
                }

                var state = _stateRepository.GetByName(placeData == null ? "" : placeData.StateName);
                if (state == null)
                {
                    state = _stateRepository.Save(new Contracts.DataModels.State
                    {
                        AltId = Guid.NewGuid(),
                        Name = placeData == null ? "" : placeData.StateName,
                        Abbreviation = string.Empty,
                        CountryId = country.Id,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid(),
                        UpdatedBy = Guid.NewGuid()
                    });
                }
                else
                {
                    state.CountryId = country.Id;
                    state.UpdatedUtc = DateTime.UtcNow;
                    state.UpdatedBy = Guid.NewGuid();
                    _stateRepository.Save(state);
                }

                var city = _cityRepository.GetByNameAndStateId(cityName, state.Id);
                if (city == null)
                {
                    city = _cityRepository.Save(new Contracts.DataModels.City
                    {
                        AltId = Guid.NewGuid(),
                        Name = cityName,
                        StateId = state.Id,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid(),
                        UpdatedBy = Guid.NewGuid()
                    });
                }
                else
                {
                    city.StateId = state.Id;
                    city.UpdatedUtc = DateTime.UtcNow;
                    city.UpdatedBy = Guid.NewGuid();
                    _cityRepository.Save(city);
                }

                var pOneVenue = _pOneVenueRepository.GetByName(pOneEvent.languages.EN.location);
                if (pOneVenue == null)
                {
                    pOneVenue = _pOneVenueRepository.Save(new POneVenue
                    {
                        Name = pOneEvent.languages.EN.location == "unknown" && placeData.Name != "" ? placeData.Name : pOneEvent.languages.EN.location,
                        Address = pOneEvent.formatted_address,
                        CityId = city.Id,
                        Latitude = pOneEvent.lat,
                        Longitude = pOneEvent.lng,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    pOneVenue.Address = pOneEvent.formatted_address;
                    pOneVenue.CityId = city.Id;
                    pOneVenue.Latitude = pOneEvent.lat;
                    pOneVenue.Longitude = pOneEvent.lng;
                    pOneVenue.UpdatedUtc = DateTime.UtcNow;
                    _pOneVenueRepository.Save(pOneVenue);
                }

                var pOneEventDetailName = GetMatchName(pOneEvent);

                var pOneEventDetail = _pOneEventDetailRepository.GetByPOneId(Convert.ToInt32(pOneEvent.languages.EN.match_id));
                if (pOneEventDetail == null)
                {
                    pOneEventDetail = _pOneEventDetailRepository.Save(new POneEventDetail
                    {
                        POneId = Convert.ToInt32(pOneEvent.languages.EN.match_id),
                        Name = pOneEventDetailName,
                        POneEventId = @event.POneId,
                        POneVenueId = pOneVenue.Id,
                        StartDateTime = Convert.ToDateTime($"{pOneEvent.date} {pOneEvent.time}"),
                        MetaDetails = pOneEvent.languages.EN.hospitality_description,
                        Description = pOneEvent.languages.EN.seating_description,
                        DeliveryTypeId = GetShippingMethod(pOneEvent.shipping_method),
                        DeliveryNotes = pOneEvent.languages.EN.delivery_description,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    pOneEventDetail.Name = pOneEventDetailName;
                    pOneEventDetail.POneVenueId = pOneVenue.Id;
                    pOneEventDetail.StartDateTime = Convert.ToDateTime($"{pOneEvent.date} {pOneEvent.time}");
                    pOneEventDetail.MetaDetails = pOneEvent.languages.EN.hospitality_description;
                    pOneEventDetail.Description = pOneEvent.languages.EN.seating_description;
                    pOneEventDetail.DeliveryTypeId = GetShippingMethod(pOneEvent.shipping_method);
                    pOneEventDetail.DeliveryNotes = pOneEvent.languages.EN.delivery_description;
                    pOneEventDetail.UpdatedUtc = DateTime.UtcNow;
                    pOneEventDetail = _pOneEventDetailRepository.Save(pOneEventDetail);
                }

                var pOneTicketCategory = _pOneTicketCategoryRepository.GetByPOneId(Convert.ToInt32(pOneEvent.languages.EN.seating_id));
                if (pOneTicketCategory == null)
                {
                    pOneTicketCategory = _pOneTicketCategoryRepository.Save(new POneTicketCategory
                    {
                        POneId = Convert.ToInt32(pOneEvent.languages.EN.seating_id),
                        Name = pOneEvent.languages.EN.seating,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }

                var pOneEventTicketDetail = _pOneEventTicketDetailRepository.GetByEventDetailIdAndTicketCategoryId(pOneEventDetail.POneId, pOneTicketCategory.POneId);
                if (pOneEventTicketDetail == null)
                {
                    pOneEventTicketDetail = _pOneEventTicketDetailRepository.Save(new POneEventTicketDetail
                    {
                        POneTicketCategoryId = pOneTicketCategory.POneId,
                        POneEventDetailId = pOneEventDetail.POneId,
                        ImageUrl = pOneEvent.image,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }

                var pOneEventTicketAttribute = _pOneEventTicketAttributeRepository.GetByPOneEventTicketDetailId(pOneEventTicketDetail.Id);
                if (pOneEventTicketAttribute == null)
                {
                    pOneEventTicketAttribute = _pOneEventTicketAttributeRepository.Save(new POneEventTicketAttribute
                    {
                        POneId = Convert.ToInt64(pOneEvent.sku),
                        POneEventTicketDetailId = pOneEventTicketDetail.Id,
                        AvailableTicketForSale = Convert.ToInt32(pOneEvent.stock),
                        TicketCategoryDescription = pOneEvent.languages.EN.seating_description,
                        Price = Convert.ToDecimal(pOneEvent.price),
                        ShippingCharge = Convert.ToDecimal(pOneEvent.shipping_costs),
                        IsDateConfirmed = Convert.ToBoolean(Convert.ToInt16(pOneEvent.datetime_confirmed)),
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    pOneEventTicketAttribute.AvailableTicketForSale = Convert.ToInt32(pOneEvent.stock);
                    pOneEventTicketAttribute.TicketCategoryDescription = pOneEvent.languages.EN.seating_description;
                    pOneEventTicketAttribute.Price = Convert.ToDecimal(pOneEvent.price);
                    pOneEventTicketAttribute.ShippingCharge = Convert.ToDecimal(pOneEvent.shipping_costs);
                    pOneEventTicketAttribute.IsDateConfirmed = Convert.ToBoolean(Convert.ToInt16(pOneEvent.datetime_confirmed));
                    pOneEventTicketAttribute.UpdatedUtc = DateTime.UtcNow;
                    pOneEventTicketAttribute = _pOneEventTicketAttributeRepository.Save(pOneEventTicketAttribute);
                }
            }
        }

        private DeliveryTypes GetShippingMethod(string pOneShipping)
        {
            DeliveryTypes shippingMethod = DeliveryTypes.MTicket;

            if (pOneShipping.Contains("Box"))
            {
                shippingMethod = DeliveryTypes.VenuePickup;
            }
            else if (pOneShipping.Contains("E-Ticket"))
            {
                shippingMethod = DeliveryTypes.ETicket;
            }
            else if (pOneShipping.Contains("M-Ticket"))
            {
                shippingMethod = DeliveryTypes.MTicket;
            }
            else if (pOneShipping.Contains("Hotel"))
            {
                shippingMethod = DeliveryTypes.HotelDelivery;
            }
            else if (pOneShipping.Contains("Standard"))
            {
                shippingMethod = DeliveryTypes.Courier;
            }
            else
            {
                shippingMethod = DeliveryTypes.MTicket;
            }

            return shippingMethod;
        }

        private string GetMatchName(SkuModel skuModel)
        {
            if (skuModel.languages.EN.category == "Concerts Hospitality")
            {
                return $"{skuModel.languages.EN.name} At {skuModel.languages.EN.event_guest}";
            }
            else if (skuModel.languages.EN.category == "Football" || skuModel.languages.EN.category == "Rugby Six Nations" || skuModel.languages.EN.category == "Other Events")
            {
                return $"{skuModel.languages.EN.@event} vs {skuModel.languages.EN.event_guest} - {skuModel.languages.EN.sub_category}";
            }
            else if (skuModel.languages.EN.category == "Motorsports" || skuModel.languages.EN.category == "Olympics Tokyo 1")
            {
                return $"{skuModel.languages.EN.name}";
            }
            else if (skuModel.languages.EN.category == "Tennis Hospitality")
            {
                return skuModel.languages.EN.event_guest != "" ? $"{skuModel.languages.EN.@event} - {skuModel.languages.EN.event_guest}" : skuModel.languages.EN.@event;
            }
            else
            {
                return $"{skuModel.languages.EN.@event} vs {skuModel.languages.EN.event_guest} - {skuModel.languages.EN.sub_category}";
            }
        }

        private string GetEventName(SkuModel skuModel)
        {
            if (skuModel.languages.EN.category == "Concerts Hospitality")
            {
                return $"{skuModel.languages.EN.name} At {skuModel.languages.EN.event_guest}";
            }
            else if (skuModel.languages.EN.category == "Rugby Six Nations" || skuModel.languages.EN.category == "Olympics Tokyo 1")
            {
                return $"{skuModel.languages.EN.category}";
            }
            else if (skuModel.languages.EN.category == "Other Events" || skuModel.languages.EN.category == "Football")
            {
                return $"{skuModel.languages.EN.name}";
            }
            else if (skuModel.languages.EN.category == "Tennis Hospitality")
            {
                return $"{skuModel.languages.EN.sub_category}";
            }
            else
            {
                return skuModel.languages.EN.@event;
            }
        }
    }
}