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
            var FilEventTicketDetails = _eventTicketDetailRepository.GetByIds(eventTicketDetailIds);

            var ticketCategoryIds = FilEventTicketDetails.Select(w => w.TicketCategoryId).ToList();
            var FilTicketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(ticketCategoryIds);

            foreach (var item in command.OptionList)
            {
                int optionCount = command.OptionList.Where(w => w.SessionId == item.SessionId).ToList().Count;
                try
                {
                    string TicketCategoryName = GetTicketCategoryName(item, optionCount);

                    ExOzProductOption exOzProductOption = exOzProductOptions.Where(w => w.ProductOptionId == item.Id).FirstOrDefault();
                    ExOzProductSession exOzProductSession = _exOzProductSessionRepository.GetBySessionId(item.SessionId);
                    ExOzProduct exOzProduct = _exOzProductRepository.Get(exOzProductSession.ProductId);

                    EventTicketDetail FilEventTicketDetail = FilEventTicketDetails.Where(w => w.Id == exOzProductOption.EventTicketDetailId).FirstOrDefault();
                    TicketCategory FilTicketCategory = FilTicketCategories.Where(w => w.Name == TicketCategoryName).FirstOrDefault();

                    TicketCategory retTicketCategory = UpdateTicketCategory(TicketCategoryName, FilTicketCategory, command.ModifiedBy);

                    EventTicketDetail retEventTicketDetail = UpdateEventTicketDetail(FilEventTicketDetail, exOzProduct.EventDetailId, retTicketCategory.Id, command.ModifiedBy);

                    EventTicketAttribute eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(retEventTicketDetail.Id);
                    EventTicketAttribute FilTicketAttribute = UpdateEventTicketAttribute(item, eventTicketAttribute, retEventTicketDetail.Id, command.ModifiedBy);

                    TicketFeeDetail ticketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeId(FilTicketAttribute.EventTicketDetailId);
                    TicketFeeDetail FilTicketFeedDetail = UpdateTicketFeeDetails(ticketFeeDetail, FilTicketAttribute.Id, command.ModifiedBy);

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

        protected TicketCategory UpdateTicketCategory(string name, TicketCategory FilTicketCategory, Guid ModifiedBy)
        {
            TicketCategory FilTicketCategoryInserted = new TicketCategory();
            if (FilTicketCategory == null)
            {
                var newFilTicketCategory = new TicketCategory
                {
                    Name = name,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                FilTicketCategoryInserted = _ticketCategoryRepository.Save(newFilTicketCategory);
            }
            else
            {
                FilTicketCategoryInserted = FilTicketCategory;
            }
            return FilTicketCategoryInserted;
        }

        protected EventTicketDetail UpdateEventTicketDetail(EventTicketDetail FilEventTicketDetail, long FilEventDetailId, long FilTicketCategoryId, Guid ModifiedBy)
        {
            EventTicketDetail FilEventTicketDetailInserted = new EventTicketDetail();
            if (FilEventTicketDetail == null)
            {
                var newFilEventTicketDetail = new EventTicketDetail
                {
                    EventDetailId = FilEventDetailId,
                    TicketCategoryId = FilTicketCategoryId,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                FilEventTicketDetailInserted = _eventTicketDetailRepository.Save(newFilEventTicketDetail);
            }
            else
            {
                FilEventTicketDetailInserted = FilEventTicketDetail;
            }
            return FilEventTicketDetailInserted;
        }

        protected EventTicketAttribute UpdateEventTicketAttribute(ExOzProductOptionResponse item, EventTicketAttribute FilEventTicketAttribute, long FilEventTicketDetailId, Guid ModifiedBy)
        {
            EventTicketAttribute FilEventTicketAttributeInserted = new EventTicketAttribute();
            if (FilEventTicketAttribute == null)
            {
                var newFilEventTicketAttribute = new EventTicketAttribute
                {
                    EventTicketDetailId = FilEventTicketDetailId,
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
                FilEventTicketAttributeInserted = _eventTicketAttributeRepository.Save(newFilEventTicketAttribute);
            }
            else
            {
                FilEventTicketAttributeInserted = FilEventTicketAttribute;
            }
            return FilEventTicketAttributeInserted;
        }

        protected TicketFeeDetail UpdateTicketFeeDetails(TicketFeeDetail FilTicketFeeDetail, long FilEventTicketAttributeId, Guid ModifiedBy)
        {
            TicketFeeDetail FilTicketFeeDetailInserted = new TicketFeeDetail();
            if (FilTicketFeeDetail == null)
            {
                var newFilTicketFeeDetail = new TicketFeeDetail
                {
                    EventTicketAttributeId = FilEventTicketAttributeId,
                    FeeId = 4,
                    DisplayName = "Bank",
                    ValueTypeId = 1,
                    Value = 0.8m,
                    FeeGroupId = null,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                FilTicketFeeDetailInserted = _ticketFeeDetailRepository.Save(newFilTicketFeeDetail);
            }
            else
            {
                FilTicketFeeDetailInserted = FilTicketFeeDetail;
            }
            return FilTicketFeeDetailInserted;
        }
    }
}