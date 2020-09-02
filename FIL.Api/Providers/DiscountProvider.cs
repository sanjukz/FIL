using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using System;

namespace FIL.Api.Providers
{
    public interface IDiscountProvider
    {
        EventTicketDiscountDetail SaveEventDiscount(FIL.Contracts.Models.CreateEventV1.DiscountModel discountModel);
    }

    public class DiscountProvider : IDiscountProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        private readonly IDiscountPromoCodeRepository _discountPromoCodeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IEventTicketDiscountDetailRepository _eventTicketDiscountDetailRepository;

        public DiscountProvider(
            FIL.Logging.ILogger logger,
            IDiscountPromoCodeRepository discountPromoCodeRepository,
            IDiscountRepository discountRepository,
            IEventTicketDiscountDetailRepository eventTicketDiscountDetailRepository
            )
        {
            _logger = logger;
            _discountPromoCodeRepository = discountPromoCodeRepository;
            _discountRepository = discountRepository;
            _eventTicketDiscountDetailRepository = eventTicketDiscountDetailRepository;
        }

        private EventTicketDiscountDetail SaveEventTicketDiscount(FIL.Contracts.Models.CreateEventV1.DiscountModel discountModel, int discountId)
        {
            var eventTicketDiscountDetail = _eventTicketDiscountDetailRepository.GetByEventTicketAttributeId((int)discountModel.EventTicketAttributeId);
            var eventTicketDiscountDetail1 = new FIL.Contracts.DataModels.EventTicketDiscountDetail()
            {
                Id = eventTicketDiscountDetail != null ? eventTicketDiscountDetail.Id : 0,
                DiscountId = discountId,
                StartDateTime = DateTime.UtcNow,
                EventTicketAttributeId = discountModel.EventTicketAttributeId,
                EndDateTime = DateTime.UtcNow,
                IsEnabled = discountModel.IsEnabled,
                CreatedBy = eventTicketDiscountDetail != null ? eventTicketDiscountDetail.CreatedBy : discountModel.ModifiedBy,
                CreatedUtc = eventTicketDiscountDetail != null ? eventTicketDiscountDetail.CreatedUtc : DateTime.UtcNow,
                ModifiedBy = discountModel.ModifiedBy,
                UpdatedBy = discountModel.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            return _eventTicketDiscountDetailRepository.Save(eventTicketDiscountDetail1);
        }

        private DiscountPromoCode SaveDiscountPromoCode(FIL.Contracts.Models.CreateEventV1.DiscountModel discountModel, int discountId)
        {
            var discountPromoCode = _discountPromoCodeRepository.Get(discountId);
            var discountPromoCodeDataModel = new FIL.Contracts.DataModels.DiscountPromoCode()
            {
                Id = discountPromoCode != null ? discountPromoCode.Id : 0,
                DiscountId = discountId,
                PromoCode = discountModel.PromoCode,
                PromoCodeStatusId = Contracts.Enums.PromoCodeStatus.Available,
                IsEnabled = discountModel.IsEnabled,
                CreatedBy = discountPromoCode != null ? discountPromoCode.CreatedBy : discountModel.ModifiedBy,
                CreatedUtc = discountPromoCode != null ? discountPromoCode.CreatedUtc : DateTime.UtcNow,
                ModifiedBy = discountModel.ModifiedBy,
                UpdatedBy = discountModel.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            return _discountPromoCodeRepository.Save(discountPromoCodeDataModel);
        }

        private Discount SaveDiscount(FIL.Contracts.Models.CreateEventV1.DiscountModel discountModel)
        {
            var EventTicketDisocuntDetail = _eventTicketDiscountDetailRepository.GetByETAId(discountModel.EventTicketAttributeId);
            var discountDataModel = new Discount()
            {
                Id = EventTicketDisocuntDetail != null ? EventTicketDisocuntDetail.DiscountId : 0,
                DiscountTypeId = discountModel.DiscountTypes,
                DiscountValueTypeId = discountModel.DiscountValueType,
                DiscountValue = discountModel.DiscountValueType == Contracts.Enums.DiscountValueType.Flat ? discountModel.DiscountAmount : discountModel.DiscountPercentage,
                MaximumDiscountPerTransaction = 0,
                OverallMaximumDiscount = 0,
                MinimumQuantityPerTransaction = 0,
                MaximumQuantityPerTransaction = 0,
                Name = discountModel.EventName,
                IsEnabled = discountModel.IsEnabled,
                OverallMaximumQuantity = 0,
                CreatedBy = EventTicketDisocuntDetail != null ? EventTicketDisocuntDetail.CreatedBy : discountModel.ModifiedBy,
                CreatedUtc = EventTicketDisocuntDetail != null ? EventTicketDisocuntDetail.CreatedUtc : DateTime.UtcNow,
                Description = "",
                ModifiedBy = discountModel.ModifiedBy,
                UpdatedBy = discountModel.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            return _discountRepository.Save(discountDataModel);
        }

        public EventTicketDiscountDetail SaveEventDiscount(FIL.Contracts.Models.CreateEventV1.DiscountModel discountModel)
        {
            try
            {
                var discount = SaveDiscount(discountModel);
                var discountPromoCode = SaveDiscountPromoCode(discountModel, discount.Id);
                var eventTicketDiscountDetail = SaveEventTicketDiscount(discountModel, discount.Id);
                return eventTicketDiscountDetail;
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return new EventTicketDiscountDetail { };
            }
        }
    }
}