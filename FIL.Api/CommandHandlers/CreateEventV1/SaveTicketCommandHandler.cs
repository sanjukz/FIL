using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SaveTicketCommandHandler : BaseCommandHandlerWithResult<CreateTicketCommand, CreateTicketCommandResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly IEventStripeAccountMappingRepository _eventStripeAccountMappingRepository;
        private readonly IDonationDetailRepository _donationDetailRepository;
        private readonly IDiscountProvider _discountProvider;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public SaveTicketCommandHandler(
             IEventDetailRepository eventDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            IEventStripeAccountMappingRepository eventStripeAccountMappingRepository,
            IDiscountProvider discountProvider,
            IDonationDetailRepository donationDetailRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _eventStripeAccountMappingRepository = eventStripeAccountMappingRepository;
            _discountProvider = discountProvider;
            _stepProvider = stepProvider;
            _donationDetailRepository = donationDetailRepository;
            _logger = logger;
        }

        public void SaveDonationDetail(CreateTicketCommand command)
        {
            // Donation Categories
            var donationTicket = command.Tickets.Where(s => s.TicketCategoryId == 19452 || s.TicketCategoryId == 12259).FirstOrDefault();
            if (donationTicket != null)
            {
                var donationDetail = new DonationDetail();
                var donationDetail1 = _donationDetailRepository.GetByEventId(command.EventId);
                donationDetail.Id = donationDetail1 != null ? donationDetail1.Id : 0;
                donationDetail.DonationAmount1 = donationTicket.DonationAmount1;
                donationDetail.DonationAmount2 = donationTicket.DonationAmount2;
                donationDetail.DonationAmount3 = donationTicket.DonationAmount3;
                donationDetail.EventId = command.EventId;
                donationDetail.IsFreeInput = true;
                donationDetail.CreatedUtc = DateTime.UtcNow;
                donationDetail.CreatedBy = Guid.NewGuid();
                donationDetail.IsEnabled = true;
                _donationDetailRepository.Save(donationDetail);
            }
        }

        public FIL.Contracts.Enums.StripeAccount GetStripeAccount(long currencyId)
        {
            if (currencyId == 1)
            {
                return FIL.Contracts.Enums.StripeAccount.StripeAustralia;
            }
            else if (currencyId == 7)
            {
                return FIL.Contracts.Enums.StripeAccount.StripeIndia;
            }
            else
            {
                return FIL.Contracts.Enums.StripeAccount.StripeUSA;
            }
        }

        public void SaveEventStripeMappings(long eventId, long currencyId)
        {
            var eventStripeMaster = new EventStripeAccountMapping();
            var eventStripeAccountMappings = _eventStripeAccountMappingRepository.GetByEventId(eventId);
            eventStripeMaster.Id = eventStripeAccountMappings != null ? eventStripeAccountMappings.Id : 0;
            eventStripeMaster.Channeld = Channels.Feel;
            eventStripeMaster.SiteId = Contracts.Enums.Site.ComSite;
            eventStripeMaster.EventId = eventId;
            eventStripeMaster.CreatedUtc = DateTime.UtcNow;
            eventStripeMaster.CreatedBy = Guid.NewGuid();
            eventStripeMaster.StripeAccountId = GetStripeAccount(currencyId);
            eventStripeMaster.IsEnabled = true;
            _eventStripeAccountMappingRepository.Save(eventStripeMaster);
        }

        public void SaveEventticketDetailTicketCategoryTypeMappings(CreateTicketCommand command, FIL.Contracts.Models.CreateEventV1.TicketModel ticketModel)
        {
            try
            {
                EventTicketDetailTicketCategoryTypeMapping eventTicketDetailTicketCategoryTypeMapping = new EventTicketDetailTicketCategoryTypeMapping();
                eventTicketDetailTicketCategoryTypeMapping.EventTicketDetailId = ticketModel.ETDId;
                eventTicketDetailTicketCategoryTypeMapping.TicketCategoryTypeId = ticketModel.TicketCategoryTypeId;
                eventTicketDetailTicketCategoryTypeMapping.TicketCategorySubTypeId = 1;
                eventTicketDetailTicketCategoryTypeMapping.UpdatedUtc = DateTime.UtcNow;
                eventTicketDetailTicketCategoryTypeMapping.IsEnabled = true;
                _eventTicketDetailTicketCategoryTypeMappingRepository.Save(eventTicketDetailTicketCategoryTypeMapping);
            }
            catch (Exception e)
            {
            }
        }

        public FIL.Contracts.DataModels.TicketCategory SaveTicketCategory(CreateTicketCommand command, FIL.Contracts.Models.CreateEventV1.TicketModel ticketModel)
        {
            var ticketCat = _ticketCategoryRepository.GetByNameAndId(ticketModel.Name, ticketModel.TicketCategoryId);
            if (ticketCat == null) // if category exists with same name
            {
                FIL.Contracts.DataModels.TicketCategory ticketCategoryObject = new FIL.Contracts.DataModels.TicketCategory();
                ticketCategoryObject.Name = ticketModel.Name;
                ticketCategoryObject.IsEnabled = true;
                ticketCategoryObject.CreatedUtc = DateTime.UtcNow;
                ticketCategoryObject.CreatedBy = command.ModifiedBy;
                ticketCategoryObject.ModifiedBy = command.ModifiedBy;
                var ticketCategory = _ticketCategoryRepository.Save(ticketCategoryObject);
                return ticketCategory;
            }
            else
            {
                return ticketCat;
            }
        }

        public EventTicketAttribute SaveEventTicketAttribute(
            CreateTicketCommand command,
            FIL.Contracts.Models.CreateEventV1.TicketModel ticketModel,
            List<FIL.Contracts.DataModels.EventTicketAttribute> eventTicketAttributes)
        {
            EventTicketAttribute eventTicketAttribute = new EventTicketAttribute();
            var currentETA = eventTicketAttributes.Where(s => s.EventTicketDetailId == ticketModel.ETDId).FirstOrDefault();
            eventTicketAttribute.Id = currentETA != null ? currentETA.Id : 0;
            eventTicketAttribute.EventTicketDetailId = ticketModel.ETDId;
            eventTicketAttribute.TicketCategoryNotes = ticketModel.TicketCategoryNotes != null ? ticketModel.TicketCategoryNotes : "";
            eventTicketAttribute.SalesStartDateTime = DateTime.UtcNow;
            eventTicketAttribute.SalesEndDatetime = DateTime.UtcNow;
            eventTicketAttribute.TicketTypeId = TicketType.Regular;
            eventTicketAttribute.ChannelId = Channels.Feel;
            eventTicketAttribute.CurrencyId = ticketModel.CurrencyId;
            eventTicketAttribute.AvailableTicketForSale = ticketModel.Quantity;
            eventTicketAttribute.RemainingTicketForSale = ticketModel.Quantity;
            eventTicketAttribute.TicketCategoryDescription = ticketModel.Description == null ? "" : ticketModel.Description;
            eventTicketAttribute.ViewFromStand = "";
            eventTicketAttribute.IsSeatSelection = false;
            eventTicketAttribute.Price = (decimal)ticketModel.Price;
            eventTicketAttribute.IsInternationalCardAllowed = true;
            eventTicketAttribute.IsEMIApplicable = false;
            eventTicketAttribute.IsEnabled = true;
            eventTicketAttribute.TicketValidityType = TicketValidityTypes.Fixed;
            eventTicketAttribute.CreatedUtc = (currentETA != null && currentETA.CreatedUtc != null) ? currentETA.CreatedUtc : DateTime.UtcNow;
            eventTicketAttribute.UpdatedUtc = DateTime.UtcNow;
            eventTicketAttribute.CreatedBy = (currentETA != null && currentETA.CreatedUtc != null) ? currentETA.CreatedBy : command.ModifiedBy;
            eventTicketAttribute.UpdatedBy = command.ModifiedBy;
            eventTicketAttribute.ModifiedBy = command.ModifiedBy;
            return _eventTicketAttributeRepository.Save(eventTicketAttribute);
        }

        public FIL.Contracts.DataModels.EventTicketDetail SaveEventTicketDetails(
            CreateTicketCommand command,
            FIL.Contracts.Models.CreateEventV1.TicketModel ticketModel,
            FIL.Contracts.DataModels.EventDetail eventDetail,
            List<FIL.Contracts.DataModels.EventTicketDetail> eventTicketDetails)
        {
            EventTicketDetail eventTicketDetail = new EventTicketDetail();
            eventTicketDetail = ticketModel.ETDId != 0 ? eventTicketDetails.Where(s => s.Id == ticketModel.ETDId).FirstOrDefault() : eventTicketDetail;
            eventTicketDetail.Id = ticketModel.ETDId == 0 ? 0 : ticketModel.ETDId;
            eventTicketDetail.AltId = ticketModel.ETDId == 0 ? Guid.NewGuid() : eventTicketDetails.Where(s => s.Id == ticketModel.ETDId).FirstOrDefault().AltId;
            eventTicketDetail.EventDetailId = eventDetail.Id;
            eventTicketDetail.TicketCategoryId = ticketModel.TicketCategoryId;
            eventTicketDetail.IsEnabled = ticketModel.isEnabled;
            eventTicketDetail.CreatedUtc = eventTicketDetail.Id != 0 ? eventTicketDetail.CreatedUtc : DateTime.UtcNow;
            eventTicketDetail.UpdatedUtc = eventTicketDetail.Id != 0 ? DateTime.UtcNow : eventTicketDetail.CreatedUtc;
            eventTicketDetail.CreatedBy = eventTicketDetail.Id != 0 ? eventTicketDetail.CreatedBy : command.ModifiedBy;
            eventTicketDetail.UpdatedBy = command.ModifiedBy;
            eventTicketDetail.ModifiedBy = command.ModifiedBy;
            return _eventTicketDetailRepository.Save(eventTicketDetail);
        }

        private void SaveTicketDiscount(CreateTicketCommand command,
            long eventTicketAttributeId,
            string EventName)
        {
            if (String.IsNullOrEmpty(command.Tickets.FirstOrDefault().PromoCode)) return;
            var discountCustomModel = new FIL.Contracts.Models.CreateEventV1.DiscountModel
            {
                DiscountAmount = command.Tickets.FirstOrDefault().DiscountAmount,
                EventTicketAttributeId = eventTicketAttributeId,
                DiscountPercentage = command.Tickets.FirstOrDefault().DiscountPercentage,
                DiscountValueType = command.Tickets.FirstOrDefault().DiscountValueType,
                DiscountTypes = FIL.Contracts.Enums.DiscountTypes.PromoCodeBased,
                EventName = EventName,
                IsEnabled = command.Tickets.FirstOrDefault().IsDiscountEnable,
                ModifiedBy = command.ModifiedBy,
                PromoCode = command.Tickets.FirstOrDefault().PromoCode
            };
            _discountProvider.SaveEventDiscount(discountCustomModel);
        }

        protected override async Task<ICommandResult> Handle(CreateTicketCommand command)
        {
            try
            {
                List<FIL.Contracts.Models.CreateEventV1.TicketModel> ticketModelList = new List<FIL.Contracts.Models.CreateEventV1.TicketModel>();
                var eventDetail = _eventDetailRepository.GetByEventId(command.EventId);
                var etdTicketCategoryMapping = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetails(command.Tickets.Select(s => s.ETDId).ToList());
                var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailId(eventDetail.Id).ToList();
                var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Select(s => s.Id).ToList()).ToList();
                var currency = _currencyTypeRepository.Get(command.Tickets.FirstOrDefault().CurrencyId);
                foreach (var currentEta in eventTicketAttributes)
                {
                    currentEta.CurrencyId = currency.Id;
                    _eventTicketAttributeRepository.Save(currentEta);
                }
                eventTicketDetails = eventTicketDetails.Where(s => command.Tickets.Any(p => p.ETDId == s.Id)).ToList();
                eventTicketAttributes = eventTicketAttributes.Where(s => command.Tickets.Any(p => p.ETDId == s.EventTicketDetailId)).ToList();
                foreach (var ticketModel in command.Tickets)
                {
                    ticketModel.CurrencyCode = currency.Code;
                    ticketModel.TicketCategoryId = SaveTicketCategory(command, ticketModel).Id;
                    var etd = SaveEventTicketDetails(command, ticketModel, eventDetail, eventTicketDetails);
                    ticketModel.ETDId = etd.Id;
                    ticketModel.TicketAltId = etd.AltId;
                    var currentEventTicketAttribute = SaveEventTicketAttribute(command, ticketModel, eventTicketAttributes);
                    SaveEventticketDetailTicketCategoryTypeMappings(command, ticketModel);
                    SaveTicketDiscount(command, currentEventTicketAttribute.Id, eventDetail.Name);
                    ticketModelList.Add(ticketModel);
                }
                SaveEventStripeMappings(command.EventId, command.Tickets.FirstOrDefault().CurrencyId);
                SaveDonationDetail(command);
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new CreateTicketCommandResult
                {
                    Tickets = ticketModelList,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new CreateTicketCommandResult { };
            }
        }
    }
}