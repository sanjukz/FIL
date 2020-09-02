using FIL.Api.Integrations;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.DataModels;
using FIL.Contracts.DataModels.Tiqets;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Tiqets;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static FIL.Contracts.Utils.Constant;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class UpdateProductCommandHandler : BaseCommandHandlerWithResult<UpdateProductCommand, UpdateProductCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITiqetProductRepository _tiqetProductRepository;
        private readonly ITiqetProductTagMappingRepository _tiqetProductTagMappingRepository;
        private readonly ITiqetTagTypeRepository _tiqetTagTypeRepository;
        private readonly ITiqetTagRepository _tiqetTagRepository;
        private readonly ITiqetProductCheckoutDetailRepository _tiqetProductCheckoutDetailRepository;
        private readonly ITiqetProductImageRepository _tiqetProductImageRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;
        private readonly ITiqetVariantDetailRepository _tiqetVariantDetailRepository;
        private readonly ITiqetEventTicketDetailMappingRepository _tiqetEventTicketDetailMappingRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidayDatesRepository;
        private readonly IGoogleMapApi _googleMapApi;
        private readonly IToEnglishTranslator _toEnglishTranslator;
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly ICountryAlphaCode _countryAlphaCode;

        public UpdateProductCommandHandler(IEventRepository eventRepository, ISettings settings, ILogger logger, ITiqetProductRepository tiqetProductRepository, ITiqetProductTagMappingRepository tiqetProductTagMappingRepository, ITiqetTagTypeRepository tiqetTagTypeRepository, ITiqetTagRepository tiqetTagRepository, ITiqetProductCheckoutDetailRepository tiqetProductCheckoutDetailRepository, ICountryRepository countryRepository,
            IStateRepository stateRepository,
            ICityRepository cityRepository, IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository, ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository, IGoogleMapApi googleMapApi, ICountryAlphaCode countryAlphaCode, ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository, ITiqetVariantDetailRepository tiqetVariantDetailRepository, ITiqetEventTicketDetailMappingRepository tiqetEventTicketDetailMappingRepository,
            IPlaceHolidayDatesRepository placeHolidayDatesRepository, ITiqetProductImageRepository tiqetProductImageRepository, IToEnglishTranslator toEnglishTranslator,
        IMediator mediator) : base(mediator)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _settings = settings;
            _tiqetProductRepository = tiqetProductRepository;
            _tiqetProductTagMappingRepository = tiqetProductTagMappingRepository;
            _tiqetTagTypeRepository = tiqetTagTypeRepository;
            _tiqetTagRepository = tiqetTagRepository;
            _tiqetProductCheckoutDetailRepository = tiqetProductCheckoutDetailRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _eventRepository = eventRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _googleMapApi = googleMapApi;
            _countryAlphaCode = countryAlphaCode;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
            _tiqetVariantDetailRepository = tiqetVariantDetailRepository;
            _tiqetEventTicketDetailMappingRepository = tiqetEventTicketDetailMappingRepository;
            _placeHolidayDatesRepository = placeHolidayDatesRepository;
            _tiqetProductImageRepository = tiqetProductImageRepository;
            _toEnglishTranslator = toEnglishTranslator;
        }

        protected override async Task<ICommandResult> Handle(UpdateProductCommand command)
        {
            UpdateProductCommandResult updateCommandResult = new UpdateProductCommandResult();
            try
            {
                List<string> imageLinks = new List<string>();
                var tiqetProduct = _tiqetProductRepository.GetByProductId(command.productId);
                var checkoutDetails = await SaveCheckoutDetails(command);
                var countryandvenuedetails = await UpdateCountryAndVenueDetails(tiqetProduct.GeoLocationLatitude, tiqetProduct.GeoLocationLongitude, tiqetProduct.VenueName, tiqetProduct.VenueAddress, command.ModifiedBy, tiqetProduct.CountryName, tiqetProduct.CityName);
                bool isImageUpload = false;
                // Saving/Updating Details to our Tables
                var events = _eventRepository.GetByEventName(tiqetProduct.Tittle);
                var formattedSlug = FormatSlug(tiqetProduct.Tittle);
                if (events == null)
                {
                    events = _eventRepository.Save(new Event
                    {
                        AltId = Guid.NewGuid(),
                        Name = tiqetProduct.Tittle,
                        EventCategoryId = 29,
                        EventTypeId = EventType.Perennial,
                        Description = null,
                        ClientPointOfContactId = 1,
                        FbEventId = null,
                        MetaDetails = null,
                        IsFeel = true,
                        EventSourceId = EventSource.Tiqets,
                        TermsAndConditions = "",
                        IsPublishedOnSite = true,
                        PublishedDateTime = DateTime.Now,
                        PublishedBy = null,
                        TestedBy = null,
                        Slug = formattedSlug,
                        ModifiedBy = command.ModifiedBy,
                        IsEnabled = true
                    });
                    isImageUpload = true;
                }
                else
                {
                    events.IsEnabled = true;
                    events.Description = tiqetProduct.Summary;
                    events.Slug = formattedSlug;
                    _eventRepository.Save(events);
                }
                if (isImageUpload || command.IsImageUpload)
                {
                    var productImages = _tiqetProductImageRepository.GetByProductId(tiqetProduct.ProductId);
                    foreach (var currentImage in productImages)
                    {
                        imageLinks.Add(currentImage.Large);
                    }
                }
                var last = _eventSiteIdMappingRepository.GetAll().OrderByDescending(p => p.CreatedUtc).FirstOrDefault();
                var eventSiteIdMapping = _eventSiteIdMappingRepository.GetByEventId(events.Id);
                if (eventSiteIdMapping == null)
                {
                    _eventSiteIdMappingRepository.Save(new Contracts.DataModels.EventSiteIdMapping
                    {
                        EventId = events.Id,
                        SortOrder = Convert.ToInt16(last.SortOrder + 1),
                        SiteId = Site.feelaplaceSite,
                        ModifiedBy = command.ModifiedBy,
                        IsEnabled = true
                    });
                }
                var getEventCategoryId = await GetCategoryId(tiqetProduct.ProductId);
                foreach (var categoryId in getEventCategoryId)
                {
                    var eventCategoryMapping = _eventCategoryMappingRepository.GetByEventIdAndEventCategoryId(events.Id, categoryId);
                    if (eventCategoryMapping == null)
                    {
                        _eventCategoryMappingRepository.Save(new Contracts.DataModels.EventCategoryMapping
                        {
                            EventId = events.Id,
                            EventCategoryId = categoryId,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventCategoryMapping.EventCategoryId = categoryId;
                        _eventCategoryMappingRepository.Save(eventCategoryMapping);
                    }
                }
                var eventDetail = _eventDetailRepository.GetByNameAndVenueId(tiqetProduct.Tittle, countryandvenuedetails.venueId);
                if (eventDetail == null)
                {
                    eventDetail = _eventDetailRepository.Save(new EventDetail
                    {
                        Name = tiqetProduct.Tittle,
                        EventId = events.Id,
                        VenueId = countryandvenuedetails.venueId,
                        StartDateTime = DateTime.UtcNow,
                        EndDateTime = DateTime.UtcNow.AddYears(1),
                        GroupId = 1,
                        AltId = Guid.NewGuid(),
                        TicketLimit = 10,
                        ModifiedBy = command.ModifiedBy,
                        IsEnabled = true,
                        MetaDetails = "",
                        HideEventDateTime = false,
                        CustomDateTimeMessage = "",
                        CreatedUtc = DateTime.UtcNow,
                        CreatedBy = command.ModifiedBy
                    });
                }
                else
                {
                    eventDetail.IsEnabled = true;
                    eventDetail.StartDateTime = DateTime.UtcNow;
                    eventDetail.EndDateTime = DateTime.UtcNow.AddYears(1);
                    eventDetail.ModifiedBy = command.ModifiedBy;
                    _eventDetailRepository.Save(eventDetail);
                }
                var eventDeliveryType = _eventDeliveryTypeDetailRepository.GetByEventDetailId(eventDetail.Id).ToList();
                if (eventDeliveryType.Count() == 0)
                {
                    _eventDeliveryTypeDetailRepository.Save(new EventDeliveryTypeDetail
                    {
                        EventDetailId = eventDetail.Id,
                        DeliveryTypeId = DeliveryTypes.MTicket,
                        Notes = "<table><tr><td valign=''top''>1.&nbsp;</td><td valign=''top''>Ticket pickup location and timing will be announced in the “Customer Update” sectionof our website closer to the event. Please check that regularly.</td></tr><tr><td valign=''top''>2.&nbsp;</td><td valign=''top''>The following documents are compulsory for ticket pickup:<br /><table><tr>  <td valign=''top''>  a.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  The card / bank account owner’s original Govt. issued photo ID, along with a clean,  fully legible, photocopy of the same ID  </td></tr><tr>  <td valign=''top''>  b.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  When a debit or credit card has been used for purchase, we also need the original  debit/credit card, along with a clean, fully legible, photocopy of the same card  </td></tr><tr>  <td valign=''top''>  c.&nbsp;  </td>  <td colspan=''2'' valign=''top''>  If sending someone else on behalf of the card holder / bank account owner, then  we need numbers 2.a. and 2.b above (originals and photocopies as mentioned) along  with the following below. This is required even if the representative’s name has  been entered into the system when buying:  </td></tr><tr>  <td valign=''top''>  </td>  <td valign=''top''>  i.&nbsp;  </td>  <td>  An authorization letter with the name of the representative, signed by the card  holder/bank account owner  </td></tr><tr>  <td valign=''top''>  </td>  <td valign=''top''>  ii.&nbsp;  </td>  <td>  A Govt issued photo ID of the representative, along with a clean and legible photocopy  of the same photo identification  </td></tr></table></td></tr><tr><td valign=''top''>3.&nbsp;</td><td valign=''top''>Please note, absence of any one of the documents above can result in the tickets being refused at the ticket pickup window</td></tr>  </table>",
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ModifiedBy = command.ModifiedBy,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow
                    });
                }

                await GetVariantDetails(tiqetProduct.ProductId);
                await DisableAllEventTicketDetailMappings(tiqetProduct.ProductId);
                var variantdetails = _tiqetVariantDetailRepository.GetAllByProductId(tiqetProduct.ProductId);
                //if variants are not available at the moment disable it.
                if (variantdetails.Count() == 0)
                {
                    events.IsEnabled = false;
                    _eventRepository.Save(events);
                    eventDetail.IsEnabled = false;
                    _eventDetailRepository.Save(eventDetail);
                    updateCommandResult.success = false;
                    return updateCommandResult;
                }
                foreach (var variantdetail in variantdetails)
                {
                    //eventTicketDetails and TicketCategories goes here
                    var ticketCategory = _ticketCategoryRepository.GetByName(variantdetail.Label);
                    if (ticketCategory == null)
                    {
                        ticketCategory = _ticketCategoryRepository.Save(new TicketCategory
                        {
                            Name = variantdetail.Label,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    var eventTicketDetail = _eventTicketDetailRepository.GetByTicketCategoryIdAndEventDetailId(ticketCategory.Id, eventDetail.Id);
                    if (eventTicketDetail == null)
                    {
                        eventTicketDetail = _eventTicketDetailRepository.Save(new EventTicketDetail
                        {
                            EventDetailId = eventDetail.Id,
                            TicketCategoryId = ticketCategory.Id,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventTicketDetail.IsEnabled = true;
                        _eventTicketDetailRepository.Save(eventTicketDetail);
                    }

                    var eventTicketDetailMapping = await SaveToTiqetsEventTicketDetailMapping(variantdetail.Id, eventTicketDetail.Id, command);

                    var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                    if (eventTicketAttribute == null)
                    {
                        eventTicketAttribute = _eventTicketAttributeRepository.Save(new EventTicketAttribute
                        {
                            EventTicketDetailId = eventTicketDetail.Id,
                            SalesStartDateTime = DateTime.UtcNow,
                            SalesEndDatetime = DateTime.UtcNow.AddYears(1),
                            TicketTypeId = TicketType.Regular,
                            ChannelId = Channels.Feel,
                            CurrencyId = TiqetsConstant.CurrencyCode,
                            SharedInventoryGroupId = null,
                            TicketCategoryDescription = "",
                            ViewFromStand = "",
                            IsSeatSelection = false,
                            AvailableTicketForSale = Convert.ToInt32(variantdetail.MaxTicketsPerOrder),
                            RemainingTicketForSale = Convert.ToInt32(variantdetail.MaxTicketsPerOrder),
                            Price = variantdetail.TotalRetailPriceInclVat,
                            IsInternationalCardAllowed = false,
                            IsEMIApplicable = false,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        eventTicketAttribute.SalesStartDateTime = DateTime.UtcNow;
                        eventTicketAttribute.SalesEndDatetime = DateTime.UtcNow.AddYears(1);
                        eventTicketAttribute.AvailableTicketForSale = Convert.ToInt32(variantdetail.MaxTicketsPerOrder);
                        eventTicketAttribute.RemainingTicketForSale = Convert.ToInt32(variantdetail.MaxTicketsPerOrder);
                        eventTicketAttribute.Price = variantdetail.TotalRetailPriceInclVat;
                        _eventTicketAttributeRepository.Save(eventTicketAttribute);
                    }
                    var ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeIdAndFeedId(eventTicketAttribute.Id, (int)FeeType.ConvenienceCharge);
                    if (ticketFeeDetail == null)
                    {
                        ticketFeeDetail = _ticketFeeDetailRepository.Save(new TicketFeeDetail
                        {
                            EventTicketAttributeId = eventTicketAttribute.Id,
                            FeeId = (int)FeeType.ConvenienceCharge,
                            DisplayName = "Convienence Charge",
                            ValueTypeId = (int)ValueTypes.Percentage,
                            Value = 5,
                            ModifiedBy = command.ModifiedBy,
                            IsEnabled = true
                        });
                    }
                }
                await DisableEventTicketDeails(tiqetProduct.ProductId);
                await SaveToEventDetailMapping(tiqetProduct.ProductId, eventDetail.Id);
                await SyncDaysAvailabilityAsync(command, eventDetail.Id, events.Id);

                updateCommandResult.IsImageUpload = imageLinks.Count() > 0 ? true : false;
                updateCommandResult.success = true;
                updateCommandResult.ImageLinks = imageLinks;
                updateCommandResult.EventAltId = events.AltId;
                return updateCommandResult;
            }
            catch (Exception e)
            {
                updateCommandResult.success = false;
                updateCommandResult.IsImageUpload = false;
                _logger.Log(LogCategory.Error, new Exception("Failed to update Product Details", e));
                return updateCommandResult;
            }
        }

        public string FormatSlug(string tittle)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false;
            foreach (char c in tittle)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                    flag = true;
                }
                else
                {
                    if (flag)
                    {
                        sb.Append("-");
                        flag = false;
                    }
                }
            }
            return sb.ToString();
        }

        public async Task SyncDaysAvailabilityAsync(UpdateProductCommand command, long eventDetailId, long eventId)
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("products/" + command.productId + "/available_days?lang=en"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
                var responseJson = Mapper<AvailableDaysReponseModel>.MapFromJson(responseData);
                if (responseJson.success && responseJson.days.Count() > 0)
                {
                    var getAllDates = await GetDatesWithinRange(responseJson.days[0], responseJson.days[responseJson.days.Count - 1]);
                    List<DateTime> responseDates = new List<DateTime>();
                    foreach (var responseJsonDates in responseJson.days)
                    {
                        responseDates.Add(DateTime.Parse(responseJsonDates).ToUniversalTime());
                    }
                    foreach (var date in getAllDates)
                    {
                        if (!responseDates.Contains(date))
                        {
                            var placeHoliDayDates = _placeHolidayDatesRepository.GetByEventandDate(eventId, date);
                            if (placeHoliDayDates == null)
                            {
                                placeHoliDayDates = _placeHolidayDatesRepository.Save(new PlaceHolidayDate
                                {
                                    LeaveDateTime = date,
                                    EventId = eventId,
                                    IsEnabled = true,
                                    EventDetailId = eventDetailId,
                                    ModifiedBy = command.ModifiedBy
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get availability details", e));
            }
        }

        private async Task<List<DateTime>> GetDatesWithinRange(string startDate, string endDate)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime StartingDate = DateTime.Parse(startDate);
            DateTime EndingDate = DateTime.Parse(endDate);
            foreach (DateTime date in GetDateRange(StartingDate, EndingDate))
            {
                dates.Add(date.ToUniversalTime());
            }
            return dates;
        }

        private static List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }

        private async Task<TiqetEventTicketDetailMapping> SaveToTiqetsEventTicketDetailMapping(long tiqetVariantId, long eventTicketDetailId, UpdateProductCommand command)
        {
            // Saving to EventTicketdetail Mapping goes here.
            try
            {
                var eventTicketDetailMapping = _tiqetEventTicketDetailMappingRepository.GetByTiqetVariantId(tiqetVariantId);
                if (eventTicketDetailMapping == null)
                {
                    eventTicketDetailMapping = _tiqetEventTicketDetailMappingRepository.Save(new TiqetEventTicketDetailMapping
                    {
                        TiqetVariantDetailId = tiqetVariantId,
                        EventTicketDetailId = eventTicketDetailId,
                        IsEnabled = true,
                        ProductId = command.productId,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    eventTicketDetailMapping.IsEnabled = true;
                    _tiqetEventTicketDetailMappingRepository.Save(eventTicketDetailMapping);
                }
                return eventTicketDetailMapping;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to save to eventticketdetail mappings", e));
                return null;
            }
        }

        public async Task<Contracts.DataModels.Tiqets.TiqetProductCheckoutDetail> SaveCheckoutDetails(UpdateProductCommand command)
        {
            try
            {
                CheckoutResponseModel checkoutdetails = new CheckoutResponseModel();
                checkoutdetails = Mapper<CheckoutResponseModel>.MapFromJson((await GetCheckoutDetails(command.productId)));
                if (checkoutdetails.success)
                {
                    var tiqetsCheckoutDetails = _tiqetProductCheckoutDetailRepository.GetByProductId(command.productId);
                    if (tiqetsCheckoutDetails == null)
                    {
                        tiqetsCheckoutDetails = _tiqetProductCheckoutDetailRepository.Save(new Contracts.DataModels.Tiqets.TiqetProductCheckoutDetail
                        {
                            ProductId = command.productId,
                            MustKnow = checkoutdetails.additional_information.must_know,
                            GoodToKnow = checkoutdetails.additional_information.good_to_know,
                            PrePurchase = checkoutdetails.additional_information.pre_purchase,
                            Usage = checkoutdetails.additional_information.usage,
                            Excluded = checkoutdetails.additional_information.excluded,
                            Included = checkoutdetails.additional_information.included,
                            PostPurchase = checkoutdetails.additional_information.post_purchase,
                            HasTimeSlot = checkoutdetails.has_timeslots,
                            HasDynamicPrice = checkoutdetails.has_dynamic_pricing,
                            IsEnabled = true,
                            CreatedBy = command.ModifiedBy,
                            CreatedUtc = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        tiqetsCheckoutDetails.MustKnow = checkoutdetails.additional_information.must_know;
                        tiqetsCheckoutDetails.GoodToKnow = checkoutdetails.additional_information.good_to_know;
                        tiqetsCheckoutDetails.PrePurchase = checkoutdetails.additional_information.pre_purchase;
                        tiqetsCheckoutDetails.Usage = checkoutdetails.additional_information.usage;
                        tiqetsCheckoutDetails.Excluded = checkoutdetails.additional_information.excluded;
                        tiqetsCheckoutDetails.Included = checkoutdetails.additional_information.included;
                        tiqetsCheckoutDetails.PostPurchase = checkoutdetails.additional_information.post_purchase;
                        tiqetsCheckoutDetails.HasTimeSlot = checkoutdetails.has_timeslots;
                        tiqetsCheckoutDetails.HasDynamicPrice = checkoutdetails.has_dynamic_pricing;
                        tiqetsCheckoutDetails.IsEnabled = true;
                        tiqetsCheckoutDetails.CreatedBy = command.ModifiedBy;
                        tiqetsCheckoutDetails.CreatedUtc = DateTime.UtcNow;
                        _tiqetProductCheckoutDetailRepository.Save(tiqetsCheckoutDetails);
                    }
                    return tiqetsCheckoutDetails;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get checkout Details", e));
                return null;
            }
        }

        public async Task DisableAllEventTicketDetailMappings(string productId)
        {
            _tiqetEventTicketDetailMappingRepository.DisableAllVariants(productId);
        }

        public async Task DisableEventTicketDeails(string productId)
        {
            var diabledEventDetailMapping = _tiqetEventTicketDetailMappingRepository.GetAllDisabled(productId);
            foreach (var id in diabledEventDetailMapping)
            {
                var eventticketDetails = _eventTicketDetailRepository.Get(id.EventTicketDetailId);
                eventticketDetails.IsEnabled = false;
                _eventTicketDetailRepository.Save(eventticketDetails);
            }
        }

        public async Task<List<int>> GetCategoryId(string productId)
        {
            List<int> eventCategoryIds = new List<int>();
            var tiqetProductTagMapping = _tiqetProductTagMappingRepository.GetByProductId(productId).Where(s => s.IsCategoryType).ToList();
            if (tiqetProductTagMapping != null && tiqetProductTagMapping.Count() > 0)
            {
                foreach (var id in tiqetProductTagMapping)
                {
                    var tiqettagsMappings = _tiqetTagRepository.GetByTagId(id.TagId);
                    eventCategoryIds.Add(tiqettagsMappings.EventCategoryId);
                }
            }
            else
            {
                // IF we do not get catgories from api then add it to the default cat.
                eventCategoryIds.Add(TiqetsConstant.DefaultEventCategoryId);
            }

            return eventCategoryIds;
        }

        public async Task<string> GetCheckoutDetails(string productId)
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("products/" + productId + "/checkout_information"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
                return responseData;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get checkout details", e));
                return null;
            }
        }

        public async Task<SaveLocationReturnValues> UpdateCountryAndVenueDetails(string lat, string lon, string venueName, string venueAddress, Guid modifiedBy, string countryName, string cityName)
        {
            try
            {
                var locationDetails = await _googleMapApi.GetLocationFromLatLong(lat, lon);

                if (locationDetails.Success)
                {
                    //check for proper required Data
                    if (!CheckValid(locationDetails))
                    {
                        var translatedVenueAddress = _toEnglishTranslator.TranslateToEnglish(venueAddress);
                        locationDetails = await _googleMapApi.GetLatLongFromAddress(translatedVenueAddress);
                        if (!CheckValid(locationDetails))
                        {
                            var translatedCityName = _toEnglishTranslator.TranslateToEnglish(cityName);
                            locationDetails = await _googleMapApi.GetLatLongFromAddress(translatedCityName);
                        }
                    }
                    var translatedResultCountryName = _toEnglishTranslator.TranslateToEnglish(locationDetails.Result.CountryName);
                    var countryCodeAndCurrency = await _countryAlphaCode.GetCountryCodeByName(translatedResultCountryName);

                    var country = _countryRepository.GetByName(translatedResultCountryName);
                    if (country == null)
                    {
                        country = _countryRepository.Save(new Country
                        {
                            Name = translatedResultCountryName,
                            IsoAlphaTwoCode = countryCodeAndCurrency == null ? countryCodeAndCurrency.Result.IsoAlphaTwoCode.ToUpper() : locationDetails.Result.CountryName.Substring(0, 2).ToUpper(),
                            IsoAlphaThreeCode = countryCodeAndCurrency == null ? countryCodeAndCurrency.Result.IsoAlphaThreeCode.ToUpper() : locationDetails.Result.CountryName.Substring(0, 3).ToUpper(),
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            ModifiedBy = modifiedBy,
                            CountryName = translatedResultCountryName
                        });
                    }
                    var translatedResultStateName = _toEnglishTranslator.TranslateToEnglish(locationDetails.Result.StateName);
                    var state = _stateRepository.GetByNameAndCountryId(translatedResultStateName, country.Id);
                    if (state == null)
                    {
                        state = _stateRepository.Save(new State
                        {
                            Name = translatedResultStateName,
                            CountryId = country.Id,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            ModifiedBy = modifiedBy
                        });
                    }
                    var translatedResultCityName = _toEnglishTranslator.TranslateToEnglish(locationDetails.Result.CityName);
                    var city = _cityRepository.GetByNameAndStateId(locationDetails.Result.CityName, state.Id);
                    if (city == null)
                    {
                        city = _cityRepository.Save(new City
                        {
                            Name = translatedResultCityName,
                            StateId = state.Id,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            ModifiedBy = modifiedBy
                        });
                    }
                    var translatedResultVenueName = _toEnglishTranslator.TranslateToEnglish(venueName);
                    var venue = _venueRepository.GetByNameAndCityId(translatedResultVenueName, city.Id);
                    if (venue == null)
                    {
                        venue = _venueRepository.Save(new Contracts.DataModels.Venue
                        {
                            AltId = Guid.NewGuid(),
                            Name = translatedResultVenueName,
                            AddressLineOne = venueAddress,
                            CityId = city.Id,
                            ModifiedBy = modifiedBy,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow
                        });
                    }
                    var values = new SaveLocationReturnValues
                    {
                        cityId = city.Id,
                        venueId = venue.Id,
                        countryId = country.Id
                    };

                    return values;
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Update Country and Venue Details", e));
            }
            return null;
        }

        public async Task GetVariantDetails(string productId)
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("products/" + productId + "/variants?lang=en"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
                var responseJson = Mapper<VariantResponseModel>.MapFromJson(responseData);
                //initially disabling all variants
                _tiqetVariantDetailRepository.DisableAllVariants(productId);

                if (responseJson.success && responseJson.variants.Count > 0)
                {
                    foreach (Variant variantDetail in responseJson.variants)
                    {
                        var tiqetVariants = _tiqetVariantDetailRepository.GetByVariantIdAndProductId(variantDetail.id, productId);
                        string valid_variantIds = "";
                        if (variantDetail.valid_with_variant_ids.Count() > 0)
                        {
                            for (int i = 0; i < variantDetail.valid_with_variant_ids.Count(); i++)
                            {
                                valid_variantIds = valid_variantIds + variantDetail.valid_with_variant_ids[i];
                                if (i < variantDetail.valid_with_variant_ids.Count() - 1)
                                {
                                    valid_variantIds = valid_variantIds + ",";
                                }
                            }
                        }
                        if (tiqetVariants == null)
                        {
                            tiqetVariants = _tiqetVariantDetailRepository.Save(new Contracts.DataModels.Tiqets.TiqetVariantDetail
                            {
                                ProductId = productId,
                                VariantId = variantDetail.id,
                                Label = variantDetail.label,
                                MaxTicketsPerOrder = variantDetail.max_tickets,
                                DistributorCommissionExclVat = Convert.ToDecimal(variantDetail.price_components_eur.distributor_commission_excl_vat),
                                TotalRetailPriceInclVat = Convert.ToDecimal(variantDetail.price_components_eur.total_retail_price_incl_vat),
                                SaleTicketValueInclVat = Convert.ToDecimal(variantDetail.price_components_eur.sale_ticket_value_incl_vat),
                                BookingFeeInclVat = Convert.ToDecimal(variantDetail.price_components_eur.booking_fee_incl_vat),
                                DynamicVariantPricing = variantDetail.dynamic_variant_pricing,
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                ValidWithVariantIds = valid_variantIds
                            });
                        }
                        else
                        {
                            tiqetVariants.VariantId = variantDetail.id;
                            tiqetVariants.Label = variantDetail.label;
                            tiqetVariants.MaxTicketsPerOrder = variantDetail.max_tickets;
                            tiqetVariants.DistributorCommissionExclVat = Convert.ToDecimal(variantDetail.price_components_eur.distributor_commission_excl_vat);
                            tiqetVariants.TotalRetailPriceInclVat = Convert.ToDecimal(variantDetail.price_components_eur.total_retail_price_incl_vat);
                            tiqetVariants.SaleTicketValueInclVat = Convert.ToDecimal(variantDetail.price_components_eur.sale_ticket_value_incl_vat);
                            tiqetVariants.BookingFeeInclVat = Convert.ToDecimal(variantDetail.price_components_eur.booking_fee_incl_vat);
                            tiqetVariants.DynamicVariantPricing = variantDetail.dynamic_variant_pricing;
                            tiqetVariants.IsEnabled = true;
                            tiqetVariants.ValidWithVariantIds = valid_variantIds;
                            _tiqetVariantDetailRepository.Save(tiqetVariants);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get variant Details", e));
            }
        }

        public async Task SaveToEventDetailMapping(string productId, long eventDetailId)
        {
            var tiqetEventDetailMappings = _tiqetEventDetailMappingRepository.GetByProductId(productId);
            if (tiqetEventDetailMappings == null)
            {
                _tiqetEventDetailMappingRepository.Save(new TiqetEventDetailMapping
                {
                    ProductId = productId,
                    EventDetailId = eventDetailId,
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow
                });
            }
            else
            {
                tiqetEventDetailMappings.IsEnabled = true;
                _tiqetEventDetailMappingRepository.Save(tiqetEventDetailMappings);
            }
        }

        public static bool CheckValid(IResponse<GoogleMapApiResponse> locationDetails)
        {
            if (locationDetails.Result.CityName == "" || locationDetails.Result.CityName == null || locationDetails.Result.CountryName == "" || locationDetails.Result.CountryName == null || locationDetails.Result.StateName == "" || locationDetails.Result.StateName == null
                        || string.IsNullOrEmpty(locationDetails.Result.CityName) || string.IsNullOrEmpty(locationDetails.Result.CountryName) || string.IsNullOrEmpty(locationDetails.Result.StateName) || string.IsNullOrWhiteSpace(locationDetails.Result.CountryName) || string.IsNullOrWhiteSpace(locationDetails.Result.CityName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}