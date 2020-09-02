using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class DeleteTicketCommandHandler : BaseCommandHandlerWithResult<DeleteTicketCommand, DeleteTicketCommandResult>
    {
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly IEventTicketDiscountDetailRepository _eventTicketDiscountDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public DeleteTicketCommandHandler(
            ITicketCategoryRepository ticketCategoryRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository,
            IEventTicketDiscountDetailRepository eventTicketDiscountDetailRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _eventTicketDiscountDetailRepository = eventTicketDiscountDetailRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(DeleteTicketCommand command)
        {
            try
            {
                var eventTicketDetail = _eventTicketDetailRepository.GetByAltId(command.ETDAltId);
                var etdTicketCategoryMapping = _eventTicketDetailTicketCategoryTypeMappingRepository.GeAlltByEventTicketDetail(eventTicketDetail.Id);
                var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                var ticketFeelDetails = _ticketFeeDetailRepository.GetAllByEventTicketAttributeId(eventTicketAttribute.Id);
                var eventTicketDetailDiscountDetail = _eventTicketDiscountDetailRepository.GetAllByEventTicketAttributeId(eventTicketAttribute.Id);
                var transactionDetails = _transactionDetailRepository.GetByEventTicketAttributeId(eventTicketAttribute.Id);

                if (!transactionDetails.Any())
                {
                    foreach (var etdTicketCat in etdTicketCategoryMapping)
                    {
                        _eventTicketDetailTicketCategoryTypeMappingRepository.Delete(etdTicketCat);
                    }

                    foreach (var ticketFeeDetail in ticketFeelDetails)
                    {
                        _ticketFeeDetailRepository.Delete(ticketFeeDetail);
                    }

                    foreach (var etdDiscount in eventTicketDetailDiscountDetail)
                    {
                        _eventTicketDiscountDetailRepository.Delete(etdDiscount);
                    }

                    if (eventTicketAttribute != null)
                    {
                        _eventTicketAttributeRepository.Delete(eventTicketAttribute);
                    }

                    if (eventTicketDetail != null)
                    {
                        _eventTicketDetailRepository.Delete(eventTicketDetail);
                    }

                    var eventStepDetails = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, command.TicketLength == 1 ? false : true, command.ModifiedBy);
                    return new DeleteTicketCommandResult
                    {
                        Success = true,
                        CompletedStep = eventStepDetails.CompletedStep,
                        CurrentStep = eventStepDetails.CurrentStep,
                    };
                }
                else
                {
                    return new DeleteTicketCommandResult { IsTicketSold = true };
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new DeleteTicketCommandResult { };
            }
        }
    }
}