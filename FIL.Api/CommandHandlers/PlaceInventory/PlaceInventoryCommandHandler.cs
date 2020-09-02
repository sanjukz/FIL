using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.PlaceInventory;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.PlaceCalendar
{
    public class PlaceInventoryCommandHandler : BaseCommandHandlerWithResult<PlaceInventoryCommand, PlaceInventoryCommandCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IEventTicketDetailRepository _eventTicketDetail;
        private readonly IEventTicketAttributeRepository _eventTicketAttribute;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IRefundPolicyRepository _refundPolicyRepository;
        private readonly IPlaceTicketRedemptionDetailRepository _placeTicketRedemptionDetailRepository;
        private readonly IPlaceDocumentTypeMappingRepository _placeDocumentTypeMappingRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IPlaceCustomerDocumentTypeMappingRepository _placeCustomerDocumentTypeMappingRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly IEventCustomerInformationMappingRepository _eventCustomerInformationMappingRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IEventStripeAccountMappingRepository _eventStripeAccountMappingRepository;

        public PlaceInventoryCommandHandler(IMediator mediator,
            IEventTicketDetailRepository eventTicketDetail,
            IEventTicketAttributeRepository eventTicketAttribute,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventCustomerInformationMappingRepository eventCustomerInformationMappingRepository,
            IPlaceTicketRedemptionDetailRepository placeTicketRedemptionDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            IEventRepository eventRepository,
            IRefundPolicyRepository refundPolicyRepository,
            IPlaceCustomerDocumentTypeMappingRepository placeCustomerDocumentTypeMappingRepository,
            IPlaceDocumentTypeMappingRepository placeDocumentTypeMappingRepository,
            IPlaceWeekOffRepository placeWeekOffRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository,
            IEventStripeAccountMappingRepository eventStripeAccountMappingRepository) : base(mediator)
        {
            _mediator = mediator;
            _eventTicketDetail = eventTicketDetail;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _placeDocumentTypeMappingRepository = placeDocumentTypeMappingRepository;
            _eventRepository = eventRepository;
            _eventTicketAttribute = eventTicketAttribute;
            _refundPolicyRepository = refundPolicyRepository;
            _placeTicketRedemptionDetailRepository = placeTicketRedemptionDetailRepository;
            _placeCustomerDocumentTypeMappingRepository = placeCustomerDocumentTypeMappingRepository;
            _eventCustomerInformationMappingRepository = eventCustomerInformationMappingRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _eventStripeAccountMappingRepository = eventStripeAccountMappingRepository;
        }

        public List<FIL.Contracts.DataModels.TicketCategory> CreateTicketCategory(List<TicketCategoriesViewModel> ticketCategoriesViewModels)
        {
            List<FIL.Contracts.DataModels.TicketCategory> ticketCategories = new List<FIL.Contracts.DataModels.TicketCategory>();
            foreach (FIL.Contracts.Commands.PlaceInventory.TicketCategoriesViewModel ticketCategory in ticketCategoriesViewModels)
            {
                var IsTicketCategoryExists = _ticketCategoryRepository.GetByName(ticketCategory.categoryName);
                if (IsTicketCategoryExists == null)
                {
                    FIL.Contracts.DataModels.TicketCategory ticketCategoryObject = new FIL.Contracts.DataModels.TicketCategory();
                    ticketCategoryObject.Name = ticketCategory.categoryName;
                    ticketCategoryObject.IsEnabled = true;
                    ticketCategoryObject.CreatedUtc = DateTime.UtcNow;
                    ticketCategoryObject.CreatedBy = Guid.NewGuid();
                    var ticketCat = _ticketCategoryRepository.Save(ticketCategoryObject);
                    ticketCategories.Add(ticketCat);
                }
                else
                {
                    ticketCategories.Add(IsTicketCategoryExists);
                }
            }
            return ticketCategories;
        }

        public FIL.Contracts.DataModels.TicketCategory CreateSingleTicketCategory(string name)
        {
            var ticketCat = _ticketCategoryRepository.GetByName(name);

            if (ticketCat == null) // if category exists with same name
            {
                FIL.Contracts.DataModels.TicketCategory ticketCategoryObject = new FIL.Contracts.DataModels.TicketCategory();
                ticketCategoryObject.Name = name;
                ticketCategoryObject.IsEnabled = true;
                ticketCategoryObject.CreatedUtc = DateTime.UtcNow;
                ticketCategoryObject.CreatedBy = Guid.NewGuid();
                var ticketCategory = _ticketCategoryRepository.Save(ticketCategoryObject);
                return ticketCategory;
            }
            else
            {
                return ticketCat;
            }
        }

        public FIL.Contracts.DataModels.EventTicketDetail CreateEventTicketDetails(long eventDetailId, int ticketCatId)
        {
            EventTicketDetail eventTicketDetail = new EventTicketDetail();
            eventTicketDetail.EventDetailId = eventDetailId;
            eventTicketDetail.TicketCategoryId = ticketCatId;
            eventTicketDetail.IsEnabled = true;
            eventTicketDetail.CreatedUtc = DateTime.UtcNow;
            eventTicketDetail.CreatedBy = Guid.NewGuid();
            var data = _eventTicketDetail.Save(eventTicketDetail);
            return data;
        }

        public EventTicketAttribute CreateEventTicketAttributes
            (long currentEventTicketDetail,
            string ticketCategoryNote,
            int CurrencyId,
            int Quantity,
            string TicketCategoryDescription,
            float PricePerTicket,
            bool IsRollingTicketValidityType,
            string Days,
            string Month,
            string Year,
            DateTime TicketValidityFixDate
            )
        {
            EventTicketAttribute eventTicketAttribute = new EventTicketAttribute();
            eventTicketAttribute.EventTicketDetailId = currentEventTicketDetail;
            eventTicketAttribute.TicketCategoryNotes = ticketCategoryNote;
            eventTicketAttribute.SalesStartDateTime = DateTime.UtcNow;
            eventTicketAttribute.SalesEndDatetime = DateTime.UtcNow;
            eventTicketAttribute.TicketTypeId = TicketType.Regular;
            eventTicketAttribute.ChannelId = Channels.Feel;
            eventTicketAttribute.CurrencyId = CurrencyId;
            eventTicketAttribute.AvailableTicketForSale = Quantity;
            eventTicketAttribute.RemainingTicketForSale = Quantity;
            eventTicketAttribute.TicketCategoryDescription = (TicketCategoryDescription == null ? "" : TicketCategoryDescription);
            eventTicketAttribute.ViewFromStand = "";
            eventTicketAttribute.IsSeatSelection = false;
            eventTicketAttribute.Price = (decimal)PricePerTicket;
            eventTicketAttribute.IsInternationalCardAllowed = true;
            eventTicketAttribute.IsEMIApplicable = false;
            eventTicketAttribute.IsEnabled = true;
            eventTicketAttribute.TicketValidityType = IsRollingTicketValidityType ? TicketValidityTypes.Rolling : TicketValidityTypes.Fixed;
            eventTicketAttribute.CreatedUtc = DateTime.UtcNow;
            eventTicketAttribute.CreatedBy = Guid.NewGuid();
            if (IsRollingTicketValidityType)
            {
                eventTicketAttribute.TicketValidity = Days + "-" + Month + "-" + Year;
            }
            else
            {
                eventTicketAttribute.TicketValidity = TicketValidityFixDate.ToString();
            }
            var currentEventTicketAttribute = _eventTicketAttribute.Save(eventTicketAttribute);
            return currentEventTicketAttribute;
        }

        public EventDeliveryTypeDetail CreateEventDeliveryTypeDetail
            (long eventDetailId,
            string TermsAndCondition,
            long RefundPolicy,
            int deliveryOption)
        {
            EventDeliveryTypeDetail deliveryTypeDetail = new EventDeliveryTypeDetail();
            deliveryTypeDetail.EventDetailId = eventDetailId;
            deliveryTypeDetail.IsEnabled = true;
            deliveryTypeDetail.DeliveryTypeId = (DeliveryTypes)Enum.ToObject(typeof(DeliveryTypes), deliveryOption);
            deliveryTypeDetail.Notes = TermsAndCondition;
            deliveryTypeDetail.RefundPolicy = RefundPolicy;
            deliveryTypeDetail.EndDate = DateTime.UtcNow;
            deliveryTypeDetail.CreatedUtc = DateTime.UtcNow;
            deliveryTypeDetail.CreatedBy = Guid.NewGuid();
            var eventDeliveryTypeDetail = _eventDeliveryTypeDetailRepository.Save(deliveryTypeDetail);
            return eventDeliveryTypeDetail;
        }

        public PlaceTicketRedemptionDetail CreatePlaceTicketRedemptionDetail
            (long eventDetailId,
            string RedemptionAddress,
            string RedemptionCity,
            string RedemptionCountry,
            string RedemptionState,
            string RedemptionZipcode,
            DateTime dateTime,
            string RedemptionInstructions)
        {
            PlaceTicketRedemptionDetail placeTicketRedemptionDetail = new PlaceTicketRedemptionDetail();
            placeTicketRedemptionDetail.EventDetailId = eventDetailId;
            placeTicketRedemptionDetail.RedemptionsAddress = RedemptionAddress;
            placeTicketRedemptionDetail.RedemptionsCity = RedemptionCity;
            placeTicketRedemptionDetail.RedemptionsCountry = RedemptionCountry;
            placeTicketRedemptionDetail.RedemptionsDateTime = dateTime;
            placeTicketRedemptionDetail.RedemptionsState = RedemptionState;
            placeTicketRedemptionDetail.RedemptionsZipcode = RedemptionZipcode;
            placeTicketRedemptionDetail.RedemptionsInstructions = RedemptionInstructions;
            placeTicketRedemptionDetail.IsEnabled = true;
            placeTicketRedemptionDetail.CreatedUtc = DateTime.UtcNow;
            var placeTicketRedemption = _placeTicketRedemptionDetailRepository.Save(placeTicketRedemptionDetail);
            return placeTicketRedemption;
        }

        public PlaceCustomerDocumentTypeMapping CreatePlaceCustomerDocumentTypeMapping
            (long eventId,
            int placeDocument)
        {
            PlaceCustomerDocumentTypeMapping placeDocumentTypeMapping = new PlaceCustomerDocumentTypeMapping();
            placeDocumentTypeMapping.EventId = eventId;
            placeDocumentTypeMapping.IsEnabled = true;
            placeDocumentTypeMapping.CustomerDocumentType = (long)placeDocument;
            placeDocumentTypeMapping.CreatedUtc = DateTime.UtcNow;
            placeDocumentTypeMapping.CreatedBy = Guid.NewGuid();
            var placeCustomerDocument = _placeCustomerDocumentTypeMappingRepository.Save(placeDocumentTypeMapping);
            return placeDocumentTypeMapping;
        }

        public void UpdateEventDeliveryTypeDetail
         (long eventDetailId,
         int deliveryType,
         bool isEnable
         )
        {
            FIL.Contracts.Enums.DeliveryTypes deliveyTypeId = (FIL.Contracts.Enums.DeliveryTypes)deliveryType;

            var placeDeliveryTypeDetail = _eventDeliveryTypeDetailRepository.GetByEventDetailIdAndDocumentTypeId(eventDetailId, deliveyTypeId);
            if (placeDeliveryTypeDetail != null && isEnable)
            {
                placeDeliveryTypeDetail.IsEnabled = true;
                _eventDeliveryTypeDetailRepository.Save(placeDeliveryTypeDetail);
            }
            else if (placeDeliveryTypeDetail != null && !isEnable)
            {
                placeDeliveryTypeDetail.IsEnabled = false;
                _eventDeliveryTypeDetailRepository.Save(placeDeliveryTypeDetail);
            }
        }

        public void UpdateEventTicketDetailsAndEventTicketAttributes(int newCategoryId, List<FIL.Contracts.DataModels.EventTicketDetail> allEventTicketDetails)
        {
            foreach (FIL.Contracts.DataModels.EventTicketDetail currentEventTicketDetail in allEventTicketDetails)
            {
                var eventTicketDetail = _eventTicketDetail.GetByTicketCategoryIdAndEventTicketDetailId((long)newCategoryId, currentEventTicketDetail.Id);
                if (eventTicketDetail != null)
                {
                    var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(eventTicketDetail.Id);
                    eventTicketDetail.IsEnabled = false;
                    eventTicketAttributes.IsEnabled = false;
                    _eventTicketDetail.Save(eventTicketDetail);
                    _eventTicketAttribute.Save(eventTicketAttributes);
                }
            }
        }

        public void UpdateEventTicketDetailsAndEventTicketAttributesByEventTicketDetailId(long eventTicketDetailId)
        {
            var eventTicketDetail = _eventTicketDetail.Get(eventTicketDetailId);
            if (eventTicketDetail != null)
            {
                var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(eventTicketDetail.Id);
                eventTicketDetail.IsEnabled = false;
                _eventTicketDetail.Save(eventTicketDetail);
                if (eventTicketAttributes != null)
                {
                    eventTicketAttributes.IsEnabled = false;
                    _eventTicketAttribute.Save(eventTicketAttributes);
                }
            }
        }

        public void UpdateEventTicketAttributes(List<FIL.Contracts.DataModels.EventTicketDetail> allEventTicketDetails, int categoryId)
        {
            try
            {
                foreach (FIL.Contracts.DataModels.EventTicketDetail currentEventTicketDetail in allEventTicketDetails)
                {
                    var eventTicketDetail = _eventTicketDetail.GetByTicketCategoryIdAndEventTicketDetailId((long)categoryId, currentEventTicketDetail.Id);
                    if (eventTicketDetail != null)
                    {
                        var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(eventTicketDetail.Id);
                        eventTicketAttributes.IsEnabled = false;
                        _eventTicketAttribute.Save(eventTicketAttributes);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public void createEventTicketDetailsAndEventTicketAttributes(
            List<FIL.Contracts.DataModels.EventDetail> eventDetails,
            int ticketCategoryId,
            string ticketCategoryNote,
            int CurrencyId,
            int Quantity,
            string TicketCategoryDescription,
            float PricePerTicket,
            bool IsRollingTicketValidityType,
            string Days,
            string Month,
            string Year,
            DateTime TicketValidityFixDate,
            bool isCreateEventTicketDetails)
        {
            foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventDetails)
            {
                var eventDetail = _eventDetailRepository.Get(currentEventDetail.Id);

                if (isCreateEventTicketDetails)
                {
                    CreateEventTicketDetails(eventDetail.Id, ticketCategoryId);
                }

                var currentEventTicketDetail = _eventTicketDetail.GetByTicketCategoryIdAndeventDetailId(ticketCategoryId, eventDetail.Id).FirstOrDefault();

                var currentEventTicketAttribute = CreateEventTicketAttributes
                      (currentEventTicketDetail.Id,
                          ticketCategoryNote,
                          CurrencyId,
                          Quantity,
                          TicketCategoryDescription,
                          PricePerTicket,
                          IsRollingTicketValidityType,
                          Days,
                          Month,
                          Year,
                          TicketValidityFixDate
                      );
            }
        }

        public void UpdateCustomerDocumentTypes(
            long eventId,
            List<int> customerIdTypes
            )
        {
            try
            {
                var placeCustomerIdTypes = _placeCustomerDocumentTypeMappingRepository.GetAllByEventId(eventId).ToList();

                foreach (FIL.Contracts.DataModels.PlaceCustomerDocumentTypeMapping placeDocument in placeCustomerIdTypes)
                {
                    _placeCustomerDocumentTypeMappingRepository.Delete(placeDocument);
                }

                foreach (int customerId in customerIdTypes)
                {
                    CreatePlaceCustomerDocumentTypeMapping((long)eventId, customerId);
                }
            }
            catch (Exception e)
            {
            }
        }

        public void UpdateDeliveryTypes(
            List<FIL.Contracts.DataModels.EventDetail> eventDetails,
            string termsAndCondition,
            long refundPolicy,
            List<int> deliveryTypes
            )
        {
            try
            {
                var placeDeliveryTypes = _eventDeliveryTypeDetailRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id));

                foreach (FIL.Contracts.DataModels.EventDeliveryTypeDetail eventDeliveryDetail in placeDeliveryTypes)
                {
                    _eventDeliveryTypeDetailRepository.Delete(eventDeliveryDetail);
                }

                foreach (FIL.Contracts.DataModels.EventDetail eventDetail in eventDetails)
                {
                    foreach (int deliveryType in deliveryTypes)
                    {
                        CreateEventDeliveryTypeDetail(eventDetail.Id, termsAndCondition, refundPolicy, deliveryType);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public void UpdateTicketRedemptionDetail(
            long eventDetail,
            string RedemptionAddress,
            string RedemptionCity,
            string RedemptionCountry,
            string RedemptionState,
            string RedemptionZipcode,
            DateTime dateTime,
            string RedemptionInstructions
            )
        {
            try
            {
                var placeDeliveryTypes = _placeTicketRedemptionDetailRepository.GetAllByEventDetailId(eventDetail);

                foreach (FIL.Contracts.DataModels.PlaceTicketRedemptionDetail eventDeliveryDetail in placeDeliveryTypes)
                {
                    _placeTicketRedemptionDetailRepository.Delete(eventDeliveryDetail);
                }
                CreatePlaceTicketRedemptionDetail(eventDetail,
                     RedemptionAddress,
                     RedemptionCity,
                     RedemptionCountry,
                     RedemptionState,
                     RedemptionZipcode,
                     dateTime,
                     RedemptionInstructions);
            }
            catch (Exception e)
            {
            }
        }

        public void AddEventticketDetailTicketCategoryTypeMappings(long eventTicketDetail, long ticketCategorySubTypeId, long ticketCategoryTypeId)
        {
            try
            {
                EventTicketDetailTicketCategoryTypeMapping eventTicketDetailTicketCategoryTypeMapping = new EventTicketDetailTicketCategoryTypeMapping();
                eventTicketDetailTicketCategoryTypeMapping.EventTicketDetailId = eventTicketDetail;
                eventTicketDetailTicketCategoryTypeMapping.TicketCategoryTypeId = ticketCategoryTypeId;
                eventTicketDetailTicketCategoryTypeMapping.TicketCategorySubTypeId = ticketCategorySubTypeId;
                eventTicketDetailTicketCategoryTypeMapping.UpdatedUtc = DateTime.UtcNow;
                eventTicketDetailTicketCategoryTypeMapping.IsEnabled = true;
                _eventTicketDetailTicketCategoryTypeMappingRepository.Save(eventTicketDetailTicketCategoryTypeMapping);
            }
            catch (Exception e)
            {
            }
        }

        public void deleteETDAndETA(long eventId)
        {
            try
            {
                var eventDetails = _eventDetailRepository.GetSubeventByEventId((int)eventId);
                foreach (FIL.Contracts.DataModels.EventDetail currentED in eventDetails)
                {
                    var eventTD = _eventTicketDetail.GetByEventDetailId(currentED.Id);
                    foreach (FIL.Contracts.DataModels.EventTicketDetail currentETD in eventTD)
                    {
                        var etdTicketCategoryMapping = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetail(currentETD.Id);
                        if (etdTicketCategoryMapping != null)
                        {
                            _eventTicketDetailTicketCategoryTypeMappingRepository.Delete(etdTicketCategoryMapping);
                        }
                        var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailId(currentETD.Id);
                        if (eventTicketAttributes != null)
                        {
                            _eventTicketAttribute.Delete(eventTicketAttributes);
                        }
                        _eventTicketDetail.Delete(currentETD);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public void saveCustomerInformations(long eventId, int customerInformationId)
        {
            try
            {
                customerInformationId = customerInformationId + 1;
                EventCustomerInformationMapping eventCustomerInformationMapping = new EventCustomerInformationMapping();
                eventCustomerInformationMapping.EventId = eventId;
                eventCustomerInformationMapping.CustomerInformationId = (long)customerInformationId;
                eventCustomerInformationMapping.CreatedUtc = DateTime.UtcNow;
                eventCustomerInformationMapping.UpdatedUtc = DateTime.UtcNow;
                eventCustomerInformationMapping.CreatedBy = Guid.NewGuid();
                eventCustomerInformationMapping.IsEnabled = true;
                _eventCustomerInformationMappingRepository.Save(eventCustomerInformationMapping);
            }
            catch (Exception e)
            {
            }
        }

        public void updateEventStatus(long eventId, bool isTrue)
        {
            try
            {
                var currentEvent = _eventRepository.Get(eventId);
                currentEvent.IsEnabled = isTrue;
                currentEvent.IsCreatedFromFeelAdmin = true;
                _eventRepository.Save(currentEvent);
            }
            catch (Exception e)
            {
            }
        }

        public void deleteCustomerInformation(long eventId)
        {
            try
            {
                var eventCustomerMapping = _eventCustomerInformationMappingRepository.GetAllByEventID(eventId);
                foreach (FIL.Contracts.DataModels.EventCustomerInformationMapping eventCustomerMappings in eventCustomerMapping)
                {
                    _eventCustomerInformationMappingRepository.Delete(eventCustomerMappings);
                }
            }
            catch (Exception e)
            {
            }
        }

        public void checkETDAndETA(long eventId)
        {
            try
            {
                var eventData = _eventRepository.Get(eventId);
                var eventDetails = _eventDetailRepository.GetSubeventByEventId((int)eventData.Id);
                var eTD = _eventTicketDetail.GetAllByEventDetailIds(eventDetails.Select(s => s.Id)); // Take all ETD to consistancy

                foreach (FIL.Contracts.DataModels.EventTicketDetail currentETD in eTD)
                {
                    var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(currentETD.Id);
                    if (eventTicketAttributes != null && (!eventTicketAttributes.IsEnabled && currentETD.IsEnabled))
                    {
                        eventTicketAttributes.IsEnabled = true;
                        _eventTicketAttribute.Save(eventTicketAttributes);
                    }

                    if (eventTicketAttributes != null && (eventTicketAttributes.IsEnabled && !currentETD.IsEnabled))
                    {
                        currentETD.IsEnabled = false;
                        _eventTicketDetail.Save(currentETD);
                    }
                    if (eventTicketAttributes == null && currentETD.IsEnabled)
                    {
                        currentETD.IsEnabled = false;
                        _eventTicketDetail.Save(currentETD);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        protected override async Task<ICommandResult> Handle(PlaceInventoryCommand command)
        {
            try
            {
                var eventData = _eventRepository.GetByAltId(command.PlaceAltId);
                List<FIL.Contracts.DataModels.TicketCategory> ticketCategories = new List<FIL.Contracts.DataModels.TicketCategory>();
                List<FIL.Contracts.DataModels.EventTicketDetail> eventTicketDetails = new List<Contracts.DataModels.EventTicketDetail>();
                List<FIL.Contracts.DataModels.EventTicketAttribute> eventTicketAttributes = new List<Contracts.DataModels.EventTicketAttribute>();
                FIL.Contracts.DataModels.PlaceTicketRedemptionDetail ticketRedemptionDetails = new Contracts.DataModels.PlaceTicketRedemptionDetail();
                List<FIL.Contracts.DataModels.PlaceCustomerDocumentTypeMapping> placeCustomerDocumentTypeMappings = new List<PlaceCustomerDocumentTypeMapping>();
                List<EventDeliveryTypeDetail> eventDeliveryTypeDetails = new List<EventDeliveryTypeDetail>();

                var placeDetail = _eventDetailRepository.GetSubEventByEventId(eventData.Id).ToList();
                var currentPlaceDetail = _eventDetailRepository.GetByEvent(eventData.Id).ToList();
                if (!currentPlaceDetail.Any())
                {
                    try
                    {
                        foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in currentPlaceDetail)
                        {
                            currentEventDetail.IsEnabled = false;
                            currentEventDetail.EventId = 2384;
                            _eventDetailRepository.Save(currentEventDetail);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    EventDetail eventDetail = new EventDetail();
                    eventDetail.AltId = Guid.NewGuid();
                    eventDetail.EventId = eventData.Id;
                    eventDetail.Description = "";
                    eventDetail.IsEnabled = true;
                    eventDetail.Name = eventData.Name;
                    eventDetail.VenueId = 9893;
                    eventDetail.IsEnabled = true;
                    eventDetail.CreatedBy = command.ModifiedBy;
                    eventDetail.CreatedUtc = DateTime.Now;
                    eventDetail.UpdatedBy = command.ModifiedBy;
                    eventDetail.ModifiedBy = command.ModifiedBy;
                    eventDetail.UpdatedUtc = DateTime.Now;
                    eventDetail.StartDateTime = DateTime.Now;
                    eventDetail.EndDateTime = DateTime.UtcNow.AddYears(10);
                    eventDetail.GroupId = 1;
                    _eventDetailRepository.Save(eventDetail);
                }
                if (!command.IsEdit)
                {
                    try
                    {
                        deleteETDAndETA(eventData.Id);
                    }
                    catch (Exception e)
                    {
                    }
                    var createdTicketCategoryList = CreateTicketCategory(command.ticketCategoriesViewModels);
                    var eventDetails = _eventDetailRepository.GetSubeventByEventId((int)eventData.Id);
                    var EventDetailAltIds = eventDetails.Select(s => s.AltId).ToList();
                    if (eventData.TermsAndConditions != command.TermsAndCondition)
                    {
                        eventData.TermsAndConditions = command.TermsAndCondition;
                        _eventRepository.Save(eventData);
                    }
                    if (command.CustomerInformation.Count() > 0)
                    {
                        foreach (int customerInformationId in command.CustomerInformation)
                        {
                            saveCustomerInformations(eventData.Id, customerInformationId);
                        }
                    }

                    foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventDetails)
                    {
                        foreach (FIL.Contracts.Commands.PlaceInventory.TicketCategoriesViewModel ticketCategory in command.ticketCategoriesViewModels)
                        {
                            var ticketCat = _ticketCategoryRepository.GetByName(ticketCategory.categoryName);
                            ticketCategories.Add(ticketCat);
                            if (ticketCat != null)
                            {
                                var currentCreatedEventTicketDetail = CreateEventTicketDetails(currentEventDetail.Id, ticketCat.Id);

                                eventTicketDetails.Add(currentCreatedEventTicketDetail);

                                var currentEventTicketAttribute = CreateEventTicketAttributes
                                      (currentCreatedEventTicketDetail.Id,
                                          ticketCategory.TicketCategoryNote,
                                          ticketCategory.CurrencyId,
                                          ticketCategory.Quantity,
                                          ticketCategory.TicketCategoryDescription,
                                          ticketCategory.PricePerTicket,
                                          ticketCategory.IsRollingTicketValidityType,
                                          ticketCategory.Days,
                                          ticketCategory.Month,
                                          ticketCategory.Year,
                                          ticketCategory.TicketValidityFixDate
                                      );
                                //Save to Ticket Fee Details
                                if (command.FeeTypes != null)
                                {
                                    SaveTicketFeeDetails(currentEventTicketAttribute, command.FeeTypes, command.ModifiedBy, ticketCat.Name);
                                }
                                AddEventticketDetailTicketCategoryTypeMappings(
                                    currentCreatedEventTicketDetail.Id,
                                    ticketCategory.TicketSubCategoryTypeId,
                                    ticketCategory.TicketCategoryTypeId);
                                eventTicketAttributes.Add(currentEventTicketAttribute);
                            }
                        }
                        UpdateDeliveryTypes(eventDetails.ToList(), command.TermsAndCondition, (long)command.RefundPolicy, command.DeliverType);
                        if (command.DeliverType.Contains(2)) // if venue pickup
                        {
                            UpdateTicketRedemptionDetail(currentEventDetail.Id,
                                                    command.RedemptionAddress,
                                                    command.RedemptionCity,
                                                    command.RedemptionCountry,
                                                    command.RedemptionState,
                                                    command.RedemptionZipcode,
                                                    command.RedemptionDateTime,
                                                    command.RedemptionInstructions);
                        }
                        UpdateCustomerDocumentTypes(eventData.Id, command.CustomerIdTypes);
                    }
                }
                else
                {
                    var isEventStatusUpdate = true;
                    var eventDetails = _eventDetailRepository.GetAllByEventId(eventData.Id);
                    var allEventTicketDetails = _eventTicketDetail.GetByEventDetailIds(eventDetails.Select(s => s.Id));
                    var allEventTicketDetailIds = allEventTicketDetails.Select(s => s.Id);

                    if (command.CustomerInformation.Count() > 0)
                    {
                        deleteCustomerInformation(eventData.Id);
                        foreach (int customerInformationId in command.CustomerInformation)
                        {
                            saveCustomerInformations(eventData.Id, customerInformationId);
                        }
                    }
                    foreach (TicketCategoriesViewModel ticketCat in command.ticketCategoriesViewModels)
                    {
                        var ticketCategory = _ticketCategoryRepository.Get(ticketCat.TicketCategoryId); // if updated for same TicketCategoryId
                        int ticketCatId = 0;
                        if (ticketCategory != null) /*---- if added new category in edit so ticketCat.TicketCategoryId =0 then it's not exist ------*/
                        {
                            ticketCatId = ticketCategory.Id;
                        }
                        if (ticketCategory != null) // if ticket categoryId Exists
                        {
                            if (ticketCategory.Name != ticketCat.categoryName) // if name change
                            {
                                var category = CreateSingleTicketCategory(ticketCat.categoryName);
                                ticketCatId = category.Id; // updated ticket category id
                                UpdateEventTicketDetailsAndEventTicketAttributes(ticketCat.TicketCategoryId, allEventTicketDetails.ToList()); // update Eventticketdetails

                                createEventTicketDetailsAndEventTicketAttributes(eventDetails.ToList(),
                                          category.Id,
                                          ticketCat.TicketCategoryNote,
                                          ticketCat.CurrencyId,
                                          ticketCat.Quantity,
                                          ticketCat.TicketCategoryDescription,
                                          ticketCat.PricePerTicket,
                                          ticketCat.IsRollingTicketValidityType,
                                          ticketCat.Days,
                                          ticketCat.Month,
                                          ticketCat.Year,
                                          ticketCat.TicketValidityFixDate,
                                          true);
                            }

                            if (ticketCat.IsEventTicketAttributeUpdated && ticketCategory.Name == ticketCat.categoryName) // if eventTicketAttributeUpdated
                            {
                                UpdateEventTicketAttributes(allEventTicketDetails.ToList(), ticketCat.TicketCategoryId); // update status to 0

                                /*------------------- To check whether quantity and price updated to update event status ------------------*/
                                var currentEventTicketDetails = allEventTicketDetails.Where(s => s.TicketCategoryId == ticketCatId).FirstOrDefault();
                                if (currentEventTicketDetails != null)
                                {
                                    var currentEventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailIdFeelAdmin(currentEventTicketDetails.Id);
                                    if (currentEventTicketAttributes != null)
                                    {
                                        if ((Convert.ToInt32(currentEventTicketAttributes.Price) != Convert.ToInt32(ticketCat.PricePerTicket)) || (currentEventTicketAttributes.AvailableTicketForSale != ticketCat.Quantity))
                                        {
                                            isEventStatusUpdate = false;
                                        }
                                    }
                                }
                                /*--------------------Create eventTicketDetails and eventTicketAttrbutes -------------------*/
                                createEventTicketDetailsAndEventTicketAttributes(eventDetails.ToList(),
                                          ticketCatId,
                                          ticketCat.TicketCategoryNote,
                                          ticketCat.CurrencyId,
                                          ticketCat.Quantity,
                                          ticketCat.TicketCategoryDescription,
                                          ticketCat.PricePerTicket,
                                          ticketCat.IsRollingTicketValidityType,
                                          ticketCat.Days,
                                          ticketCat.Month,
                                          ticketCat.Year,
                                          ticketCat.TicketValidityFixDate,
                                          false);
                            }
                        }
                        else  // if new ticket category
                        {
                            var category = CreateSingleTicketCategory(ticketCat.categoryName);
                            foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventDetails)
                            {
                                var eventDetail = _eventDetailRepository.GetByAltId(currentEventDetail.AltId);

                                var currentCreatedEventTicketDetail = CreateEventTicketDetails(eventDetail.Id, category.Id);
                                eventTicketDetails.Add(currentCreatedEventTicketDetail);

                                var currentEventTicketAttribute = CreateEventTicketAttributes
                                      (currentCreatedEventTicketDetail.Id,
                                          ticketCat.TicketCategoryNote,
                                          ticketCat.CurrencyId,
                                          ticketCat.Quantity,
                                          ticketCat.TicketCategoryDescription,
                                          ticketCat.PricePerTicket,
                                          ticketCat.IsRollingTicketValidityType,
                                          ticketCat.Days,
                                          ticketCat.Month,
                                          ticketCat.Year,
                                          ticketCat.TicketValidityFixDate
                                      );
                                eventTicketAttributes.Add(currentEventTicketAttribute);
                                //Save to Ticket Fee Details
                                if (command.FeeTypes != null)
                                {
                                    SaveTicketFeeDetails(currentEventTicketAttribute, command.FeeTypes, command.ModifiedBy, category.Name);
                                }
                            }
                        }
                    }
                    /*-----------update event status ---------*/
                    if (isEventStatusUpdate)
                    {
                        updateEventStatus(eventData.Id, false);
                    }
                    foreach (long currentEventTicketDetailId in allEventTicketDetailIds)
                    {
                        var isEventTicketDetailExists = false;
                        foreach (TicketCategoriesViewModel ticketCat in command.ticketCategoriesViewModels)
                        {
                            if (ticketCat.EventTicketDetailId == currentEventTicketDetailId)
                            {
                                isEventTicketDetailExists = true;
                            }
                        }
                        if (!isEventTicketDetailExists)
                        {
                            UpdateEventTicketDetailsAndEventTicketAttributesByEventTicketDetailId(currentEventTicketDetailId);
                        }
                    }
                    UpdateTicketFeeDetails(command.FeeTypes, command.ModifiedBy, command.ticketCategoriesViewModels);
                    UpdateCustomerDocumentTypes(eventData.Id, command.CustomerIdTypes);
                    UpdateDeliveryTypes(eventDetails.ToList(), command.TermsAndCondition, (long)command.RefundPolicy, command.DeliverType);
                    foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in eventDetails)
                    {
                        if (command.DeliverType.Contains(2)) // if venue pickup
                        {
                            UpdateTicketRedemptionDetail(currentEventDetail.Id,
                                                    command.RedemptionAddress,
                                                    command.RedemptionCity,
                                                    command.RedemptionCountry,
                                                    command.RedemptionState,
                                                    command.RedemptionZipcode,
                                                    command.RedemptionDateTime,
                                                    command.RedemptionInstructions);
                        }
                    }
                }
                checkETDAndETA(eventData.Id);
                return new PlaceInventoryCommandCommandResult
                {
                    Success = true,
                };
            }
            catch (Exception e)
            {
                return new PlaceInventoryCommandCommandResult
                {
                    Success = false
                };
            }
        }

        public void SaveTicketFeeDetails(EventTicketAttribute eventTicketAttribute, List<FeeTypes> feeTypes, Guid modifiedBy, string ticketCatName)
        {
            foreach (var currentFeeType in feeTypes)
            {
                var selectedCategory = currentFeeType.CategoryName.Split(',');
                if (selectedCategory.Contains("All") || selectedCategory.Contains(ticketCatName))
                {
                    var ticketFeeDetails = _ticketFeeDetailRepository.GetByEventTicketAttributeIdAndFeedId(eventTicketAttribute.Id, Convert.ToInt16(currentFeeType.FeeTypeId));
                    if (ticketFeeDetails == null)
                    {
                        _ticketFeeDetailRepository.Save(new TicketFeeDetail
                        {
                            EventTicketAttributeId = eventTicketAttribute.Id,
                            Value = currentFeeType.Value,
                            FeeId = Convert.ToInt16(currentFeeType.FeeTypeId),
                            ValueTypeId = Convert.ToInt16(currentFeeType.ValueTypeId),
                            IsEnabled = true,
                            DisplayName = currentFeeType.FeeType,
                            ModifiedBy = modifiedBy
                        });
                    }
                    else
                    {
                        ticketFeeDetails.Value = currentFeeType.Value;
                        ticketFeeDetails.ValueTypeId = Convert.ToInt16(currentFeeType.ValueTypeId);
                        ticketFeeDetails.DisplayName = currentFeeType.FeeType;
                        _ticketFeeDetailRepository.Save(ticketFeeDetails);
                    }
                }
            }
        }

        public void UpdateTicketFeeDetails(List<FeeTypes> feeTypes, Guid modifiedBy, List<TicketCategoriesViewModel> ticketCategoriesViewModels)
        {
            //updating the existing fields
            foreach (var currentFeeType in feeTypes.Where(s => s.EventTicketAttributeId != 0))
            {
                var currentTicketFee = _ticketFeeDetailRepository.GetByEventTicketAttributeIdAndFeedId(currentFeeType.EventTicketAttributeId, Convert.ToInt16(currentFeeType.FeeTypeId));
                bool isUpdate = false;
                if (currentTicketFee.Value != currentFeeType.Value)
                {
                    currentTicketFee.Value = currentFeeType.Value;
                    isUpdate = true;
                }
                if (currentTicketFee.ValueTypeId != currentFeeType.ValueTypeId)
                {
                    currentTicketFee.ValueTypeId = currentTicketFee.ValueTypeId;
                    isUpdate = true;
                }
                if (isUpdate)
                {
                    _ticketFeeDetailRepository.Save(currentTicketFee);
                }
            }
            //looking for newly added/modified fields
            foreach (var currentTicketCat in ticketCategoriesViewModels)
            {
                var currentEventTicketAttribute = _eventTicketAttribute.GetByEventTicketDetailId(currentTicketCat.EventTicketDetailId);
                SaveTicketFeeDetails(currentEventTicketAttribute, feeTypes.ToList(), modifiedBy, currentTicketCat.categoryName);
            }
            // deleting ticket fee details entry if any
            foreach (var currentTicketCat in ticketCategoriesViewModels)
            {
                var currentEventTicketAttribute = _eventTicketAttribute.GetByEventTicketDetailId(currentTicketCat.EventTicketDetailId);
                var currentFeetypeModel = feeTypes.Where(s => s.EventTicketAttributeId == currentEventTicketAttribute.Id).ToList();
                var currentTicketFeeDetails = _ticketFeeDetailRepository.GetAllByEventTicketAttributeId(currentEventTicketAttribute.Id);
                if (currentFeetypeModel.Count == 0)
                {
                    foreach (var ticketFeeitem in currentTicketFeeDetails)
                    {
                        _ticketFeeDetailRepository.Delete(ticketFeeitem);
                    }
                }
                else
                {
                    foreach (var currentFee in currentTicketFeeDetails)
                    {
                        var feeExists = currentFeetypeModel.Where(s => s.FeeTypeId == currentFee.Id).ToList();
                        if (feeExists.Count == 0)
                        {
                            _ticketFeeDetailRepository.Delete(currentFee);
                        }
                    }
                }
            }
        }
    }
}