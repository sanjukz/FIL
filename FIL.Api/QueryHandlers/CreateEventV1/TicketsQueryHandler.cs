using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class TicketsQueryHandler : IQueryHandler<TicketQuery, TicketQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly IDiscountPromoCodeRepository _discountPromoCodeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IEventTicketDiscountDetailRepository _eventTicketDiscountDetailRepository;
        private readonly IDonationDetailRepository _donationDetailRepository;

        public TicketsQueryHandler(IEventRepository eventRepository,
             ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            IDiscountRepository discountRepository,
            IDiscountPromoCodeRepository discountPromoCodeRepository,
            IEventTicketDiscountDetailRepository eventTicketDiscountDetailRepository,
            IDonationDetailRepository donationDetailRepository,
            IEventDetailRepository eventDetailRepository)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _discountRepository = discountRepository;
            _discountPromoCodeRepository = discountPromoCodeRepository;
            _eventTicketDiscountDetailRepository = eventTicketDiscountDetailRepository;
            _donationDetailRepository = donationDetailRepository;
        }

        public TicketQueryResult Handle(TicketQuery query)
        {
            try
            {
                List<FIL.Contracts.Models.CreateEventV1.TicketModel> ticketModelList = new List<FIL.Contracts.Models.CreateEventV1.TicketModel>();
                var currentEvent = _eventRepository.Get(query.EventId);
                if (currentEvent == null)
                {
                    return new TicketQueryResult { Success = true };
                }
                var eventDetail = _eventDetailRepository.GetByEventId(query.EventId);
                if (eventDetail == null)
                {
                    return new TicketQueryResult
                    {
                        Success = true,
                        IsDraft = true,
                        IsValidLink = true,
                        Tickets = ticketModelList
                    };
                }
                var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailId(eventDetail.Id);
                var eventTicketDetailTicketCategoryMappingData = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetails(eventTicketDetails.Select(s => s.Id).ToList()).ToList();
                if (query.TicketCategoryTypeId == 2)
                {
                    eventTicketDetailTicketCategoryMappingData = eventTicketDetailTicketCategoryMappingData.Where(s => s.TicketCategoryTypeId == 2).ToList();
                    eventTicketDetails = eventTicketDetails.Where(p => eventTicketDetailTicketCategoryMappingData.Any(p2 => p2.EventTicketDetailId == p.Id) || (p.TicketCategoryId == 19452 || p.TicketCategoryId == 12259));
                    if (!eventTicketDetails.Any())
                    {
                        return new TicketQueryResult
                        {
                            EventDetailId = eventDetail.Id,
                            EventId = currentEvent.Id,
                            Success = true,
                            IsValidLink = true,
                            Tickets = ticketModelList
                        };
                    }
                }
                else
                {
                    eventTicketDetailTicketCategoryMappingData = eventTicketDetailTicketCategoryMappingData.Where(s => s.TicketCategoryTypeId != 2).ToList();
                    eventTicketDetails = eventTicketDetails.Where(p => eventTicketDetailTicketCategoryMappingData.Any(p2 => p2.EventTicketDetailId == p.Id));
                }
                var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId));
                var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Select(s => s.Id));
                var currency = _currencyTypeRepository.GetByCurrencyIds(eventTicketAttributes.Select(s => s.CurrencyId).ToList());
                var eventTicketDiscountDetails = _eventTicketDiscountDetailRepository.GetAllByEventTicketAttributeIds(eventTicketAttributes.Select(s => s.Id).ToList());
                var discounts = _discountRepository.GetAllDiscountsByIds(eventTicketDiscountDetails.Select(s => (long)s.DiscountId).ToList());
                var discountPromoCodes = _discountPromoCodeRepository.GetAllDiscountIds(discounts.Select(s => (long)s.Id).ToList());
                foreach (var currentEventTicketDetail in eventTicketDetails)
                {
                    FIL.Contracts.Models.CreateEventV1.TicketModel newTicketModel = new FIL.Contracts.Models.CreateEventV1.TicketModel();
                    var currentTicketCat = ticketCategories.Where(s => s.Id == currentEventTicketDetail.TicketCategoryId).FirstOrDefault();
                    var currentEventTicketAttribute = eventTicketAttributes.Where(s => s.EventTicketDetailId == currentEventTicketDetail.Id).FirstOrDefault();
                    var currentEventTicketDetailTicketCategoryMappingData = eventTicketDetailTicketCategoryMappingData.Where(s => s.EventTicketDetailId == currentEventTicketDetail.Id).FirstOrDefault();
                    var currentEventTicketDiscountDetails = eventTicketDiscountDetails.Where(s => s.EventTicketAttributeId == currentEventTicketAttribute.Id).FirstOrDefault();
                    var currentDiscount = currentEventTicketDiscountDetails != null ? discounts.Where(s => s.Id == currentEventTicketDiscountDetails.DiscountId).FirstOrDefault() : null;
                    var currentDiscountPromoCodes = currentDiscount != null ? discountPromoCodes.Where(s => s.DiscountId == currentDiscount.Id).FirstOrDefault() : null;
                    var donationDetail = new FIL.Contracts.DataModels.DonationDetail();
                    if (currentEventTicketDetail.TicketCategoryId == 19452 || currentEventTicketDetail.TicketCategoryId == 12259)
                    {
                        donationDetail = _donationDetailRepository.GetByEventId(query.EventId);
                    }
                    newTicketModel.TicketAltId = currentEventTicketDetail.AltId;
                    newTicketModel.ETDId = currentEventTicketDetail.Id;
                    newTicketModel.TicketCategoryId = currentTicketCat.Id;
                    newTicketModel.Name = currentTicketCat.Name;
                    newTicketModel.Price = currentEventTicketAttribute.Price;
                    newTicketModel.Quantity = currentEventTicketAttribute.AvailableTicketForSale;
                    newTicketModel.Description = currentEventTicketAttribute.TicketCategoryDescription;
                    newTicketModel.CurrencyId = currentEventTicketAttribute.CurrencyId;
                    newTicketModel.TicketCategoryTypeId = currentEventTicketDetailTicketCategoryMappingData == null ? 1 : (currentEventTicketDetailTicketCategoryMappingData != null && currentEventTicketDetailTicketCategoryMappingData.TicketCategoryTypeId == 1) ? 1 : 2;
                    newTicketModel.isEnabled = currentEventTicketDetail.IsEnabled;
                    newTicketModel.CurrencyCode = currency.Where(s => s.Id == currentEventTicketAttribute.CurrencyId).FirstOrDefault().Code;
                    newTicketModel.TotalQuantity = currentEventTicketAttribute.AvailableTicketForSale;
                    newTicketModel.RemainingQuantity = currentEventTicketAttribute.RemainingTicketForSale;
                    newTicketModel.DiscountAmount = currentDiscount != null && currentDiscount.DiscountValueTypeId == Contracts.Enums.DiscountValueType.Flat ? currentDiscount.DiscountValue : 0;
                    newTicketModel.IsDiscountEnable = currentEventTicketDiscountDetails != null ? currentEventTicketDiscountDetails.IsEnabled : false;
                    newTicketModel.DiscountPercentage = currentDiscount != null && currentDiscount.DiscountValueTypeId == Contracts.Enums.DiscountValueType.Percentage ? currentDiscount.DiscountValue : 0;
                    newTicketModel.PromoCode = currentDiscountPromoCodes != null ? currentDiscountPromoCodes.PromoCode : null;
                    newTicketModel.DiscountValueType = currentDiscount != null ? currentDiscount.DiscountValueTypeId : Contracts.Enums.DiscountValueType.Flat;
                    newTicketModel.DonationAmount1 = donationDetail != null ? donationDetail.DonationAmount1 : 0;
                    newTicketModel.DonationAmount2 = donationDetail != null ? donationDetail.DonationAmount2 : 0;
                    newTicketModel.DonationAmount3 = donationDetail != null ? donationDetail.DonationAmount3 : 0;
                    ticketModelList.Add(newTicketModel);
                }
                return new TicketQueryResult
                {
                    EventDetailId = eventDetail.Id,
                    EventId = currentEvent.Id,
                    Success = true,
                    IsValidLink = true,
                    Tickets = ticketModelList
                };
            }
            catch (Exception e)
            {
                return new TicketQueryResult { };
            }
        }
    }
}