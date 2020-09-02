using FIL.Api.Repositories;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ExOz
{
    public class SaveExOzProductpOtionCommandHandler : BaseCommandHandlerWithResult<SaveExOzProductOptionCommand, SaveExOzProductOptionCommandResult>
    {
        private readonly IExOzProductOptionRepository _exOzProductOptionRepository;
        private readonly IExOzProductSessionRepository _exOzProductSessionRepository;
        private readonly IExOzProductRepository _exOzProductRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;

        private SaveExOzProductOptionCommandResult updatedOptions = new SaveExOzProductOptionCommandResult()
        {
            OptionList = new List<ExOzProductOption>()
        };

        public SaveExOzProductpOtionCommandHandler(IExOzProductOptionRepository exOzProductOptionRepository, IExOzProductSessionRepository exOzProductSessionRepository, IExOzProductRepository exOzProductRepository, IEventTicketDetailRepository eventTicketDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, ITicketCategoryRepository ticketCategoryRepository, ITicketFeeDetailRepository ticketFeeDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzProductOptionRepository = exOzProductOptionRepository;
            _exOzProductSessionRepository = exOzProductSessionRepository;
            _exOzProductRepository = exOzProductRepository;

            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzProductOptionCommand command)
        {
            _exOzProductOptionRepository.DisableAllExOzProductOptions();

            UpdateAllOptions(command);
            return Task.FromResult<ICommandResult>(updatedOptions);
        }

        protected void UpdateAllOptions(SaveExOzProductOptionCommand command)
        {
            List<long> apiProductOptionIds = command.OptionList.Select(w => w.Id).ToList();
            var exOzProductOptions = _exOzProductOptionRepository.GetByOptionIds(apiProductOptionIds);

            var eventTicketDetailIds = exOzProductOptions.Select(w => w.EventTicketDetailId).ToList();
            var kzEventTicketDetails = _eventTicketDetailRepository.GetByIds(eventTicketDetailIds);

            var ticketCategoryIds = kzEventTicketDetails.Select(w => w.TicketCategoryId).ToList();
            var kzTicketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(ticketCategoryIds);

            foreach (var item in command.OptionList)
            {
                int optionCount = command.OptionList.Where(w => w.SessionId == item.SessionId).ToList().Count;
                try
                {
                    string TicketCategoryName = GetTicketCategoryName(item, optionCount);

                    ExOzProductOption exOzProductOption = exOzProductOptions.Where(w => w.ProductOptionId == item.Id).FirstOrDefault();
                    ExOzProductSession exOzProductSession = _exOzProductSessionRepository.GetBySessionId(item.SessionId);
                    ExOzProduct exOzProduct = _exOzProductRepository.Get(exOzProductSession.ProductId);

                    EventTicketDetail kzEventTicketDetail = kzEventTicketDetails.Where(w => w.Id == exOzProductOption.EventTicketDetailId).FirstOrDefault();
                    TicketCategory kzTicketCategory = kzTicketCategories.Where(w => w.Name == TicketCategoryName).FirstOrDefault();

                    TicketCategory retTicketCategory = UpdateTicketCategory(TicketCategoryName, kzTicketCategory, command.ModifiedBy);

                    EventTicketDetail retEventTicketDetail = UpdateEventTicketDetail(kzEventTicketDetail, exOzProduct.EventDetailId, retTicketCategory.Id, command.ModifiedBy);

                    EventTicketAttribute eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(retEventTicketDetail.Id);
                    EventTicketAttribute kzTicketAttribute = UpdateEventTicketAttribute(item, eventTicketAttribute, retEventTicketDetail.Id, command.ModifiedBy);

                    TicketFeeDetail ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeId(kzTicketAttribute.EventTicketDetailId);
                    TicketFeeDetail kzTicketFeedDetail = UpdateTicketFeeDetails(ticketFeeDetail, kzTicketAttribute.Id, command.ModifiedBy);

                    ExOzProductOption retOption = updateProductOption(item, exOzProductOption, retEventTicketDetail.Id, exOzProductSession.Id, command.ModifiedBy);
                    updatedOptions.OptionList.Add(retOption);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        protected string GetTicketCategoryName(ExOzProductOptionResponse item, int optionCount)
        {
            string TicketCategoryName = "";
            switch (optionCount)
            {
                case 0:
                    break;

                case 1:
                    TicketCategoryName = item.Name;
                    break;

                default:

                    TicketCategoryName = item.SessionName + " - " + item.Name;
                    break;
            }
            return TicketCategoryName;
        }

        protected ExOzProductOption updateProductOption(ExOzProductOptionResponse item, ExOzProductOption exOzProductOption, long eventTicketDetailId, long sessionId, Guid ModifiedBy)
        {
            ExOzProductOption exOzProductOptionInserted = new ExOzProductOption();
            if (exOzProductOption == null)
            {
                var newExOzProductOption = new ExOzProductOption
                {
                    ProductOptionId = item.Id,
                    Name = item.Name,
                    Price = Convert.ToDecimal(item.Price),
                    SessionId = sessionId,
                    EventTicketDetailId = eventTicketDetailId,
                    RetailPrice = Convert.ToDecimal(item.RetailPrice),
                    MaxQty = item.MaxQty,
                    MinQty = item.MinQty,
                    DefaultQty = item.DefaultQty,
                    Multiple = item.Multiple,
                    Weight = item.Weight,
                    IsFromPrice = item.IsFromPrice,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                exOzProductOptionInserted = _exOzProductOptionRepository.Save(newExOzProductOption);
            }
            else
            {
                exOzProductOption.IsEnabled = true;
                exOzProductOption.ModifiedBy = ModifiedBy;
                exOzProductOptionInserted = _exOzProductOptionRepository.Save(exOzProductOption);
            }
            return exOzProductOptionInserted;
        }

        protected TicketCategory UpdateTicketCategory(string name, TicketCategory kzTicketCategory, Guid ModifiedBy)
        {
            TicketCategory kzTicketCategoryInserted = new TicketCategory();
            if (kzTicketCategory == null)
            {
                var newKzTicketCategory = new TicketCategory
                {
                    Name = name,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                kzTicketCategoryInserted = _ticketCategoryRepository.Save(newKzTicketCategory);
            }
            else
            {
                kzTicketCategoryInserted = kzTicketCategory;
            }
            return kzTicketCategoryInserted;
        }

        protected EventTicketDetail UpdateEventTicketDetail(EventTicketDetail kzEventTicketDetail, long kzEventDetailId, long kzTicketCategoryId, Guid ModifiedBy)
        {
            EventTicketDetail kzEventTicketDetailInserted = new EventTicketDetail();
            if (kzEventTicketDetail == null)
            {
                var newKzEventTicketDetail = new EventTicketDetail
                {
                    EventDetailId = kzEventDetailId,
                    TicketCategoryId = kzTicketCategoryId,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                kzEventTicketDetailInserted = _eventTicketDetailRepository.Save(newKzEventTicketDetail);
            }
            else
            {
                kzEventTicketDetailInserted = kzEventTicketDetail;
            }
            return kzEventTicketDetailInserted;
        }

        protected EventTicketAttribute UpdateEventTicketAttribute(ExOzProductOptionResponse item, EventTicketAttribute kzEventTicketAttribute, long kzEventTicketDetailId, Guid ModifiedBy)
        {
            EventTicketAttribute kzEventTicketAttributeInserted = new EventTicketAttribute();
            if (kzEventTicketAttribute == null)
            {
                var newKzEventTicketAttribute = new EventTicketAttribute
                {
                    EventTicketDetailId = kzEventTicketDetailId,
                    SalesStartDateTime = DateTime.UtcNow,
                    SalesEndDatetime = DateTime.UtcNow,
                    TicketTypeId = Contracts.Enums.TicketType.Regular,
                    ChannelId = Contracts.Enums.Channels.Website,
                    CurrencyId = 1,
                    SharedInventoryGroupId = null,
                    AvailableTicketForSale = 10,
                    RemainingTicketForSale = 10,
                    TicketCategoryDescription = "Entry Only",
                    ViewFromStand = "",
                    IsSeatSelection = false,
                    Price = Convert.ToDecimal(item.Price),
                    IsInternationalCardAllowed = false,
                    IsEMIApplicable = false,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                kzEventTicketAttributeInserted = _eventTicketAttributeRepository.Save(newKzEventTicketAttribute);
            }
            else
            {
                kzEventTicketAttributeInserted = kzEventTicketAttribute;
            }
            return kzEventTicketAttributeInserted;
        }

        protected TicketFeeDetail UpdateTicketFeeDetails(TicketFeeDetail kzTicketFeeDetail, long kzEventTicketAttributeId, Guid ModifiedBy)
        {
            TicketFeeDetail kzTicketFeeDetailInserted = new TicketFeeDetail();
            if (kzTicketFeeDetail == null)
            {
                var newKzTicketFeeDetail = new TicketFeeDetail
                {
                    EventTicketAttributeId = kzEventTicketAttributeId,
                    FeeId = 4,
                    DisplayName = "Bank",
                    ValueTypeId = 1,
                    Value = 0.8m,
                    FeeGroupId = null,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                kzTicketFeeDetailInserted = _ticketFeeDetailRepository.Save(newKzTicketFeeDetail);
            }
            else
            {
                kzTicketFeeDetailInserted = kzTicketFeeDetail;
            }
            return kzTicketFeeDetailInserted;
        }
    }
}