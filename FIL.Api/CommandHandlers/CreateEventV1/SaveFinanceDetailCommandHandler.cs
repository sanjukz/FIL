using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Finance
{
    public class SaveFinanceDetailCommandHandler : BaseCommandHandlerWithResult<EventFinanceDetailCommand, EventFinanceDetailCommandResult>
    {
        private readonly IMasterFinanceDetailsRepository _masterFinanceDetails;
        private readonly IEventRepository _eventRepository;
        private readonly IStepProvider _stepProvider;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public SaveFinanceDetailCommandHandler(
            IMasterFinanceDetailsRepository masterFinanceDetails,
            IEventRepository eventRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator) : base(mediator)
        {
            _masterFinanceDetails = masterFinanceDetails;
            _mediator = mediator;
            _eventRepository = eventRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(EventFinanceDetailCommand command)
        {
            try
            {
                var events = _eventRepository.Get(command.EventId);
                var masterFinanceDetail = _masterFinanceDetails.Get(command.EventFinanceDetailModel.Id);
                var masterFinanceDetails = new FIL.Contracts.DataModels.Redemption.MasterFinanceDetails
                {
                    Id = masterFinanceDetail != null ? masterFinanceDetail.Id : 0,
                    AccountName = command.EventFinanceDetailModel.AccountName,
                    AccountNumber = command.EventFinanceDetailModel.AccountNumber,
                    AccountTypeId = command.EventFinanceDetailModel.AccountTypeId,
                    BankAccountTypeId = Contracts.Enums.BankAccountType.None,
                    BankName = command.EventFinanceDetailModel.BankName,
                    BranchCode = command.EventFinanceDetailModel.BranchCode,
                    CompanyName = command.EventFinanceDetailModel.CompanyName,
                    CountryId = command.EventFinanceDetailModel.CountryId,
                    CurrenyId = command.EventFinanceDetailModel.CurrenyId,
                    Email = command.EventFinanceDetailModel.Email,
                    FirstName = command.EventFinanceDetailModel.FirstName,
                    LastName = command.EventFinanceDetailModel.LastName,
                    PhoneCode = command.EventFinanceDetailModel.PhoneCode,
                    PhoneNumber = command.EventFinanceDetailModel.PhoneNumber,
                    RoutingNumber = "",
                    StateId = command.EventFinanceDetailModel.StateId,
                    TaxId = command.EventFinanceDetailModel.TaxId,
                    CreatedBy = masterFinanceDetail != null ? masterFinanceDetail.CreatedBy : command.ModifiedBy,
                    CreatedUtc = masterFinanceDetail != null ? masterFinanceDetail.CreatedUtc : DateTime.UtcNow,
                    EventId = events.Id,
                    ModifiedBy = command.ModifiedBy,
                    IsEnabled = true,
                    UpdatedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.UtcNow
                };
                var currentFinanceDetail = _masterFinanceDetails.Save(masterFinanceDetails);
                command.EventFinanceDetailModel.Id = currentFinanceDetail.Id;
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new EventFinanceDetailCommandResult
                {
                    Success = true,
                    EventAltId = events.AltId,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    EventFinanceDetailModel = command.EventFinanceDetailModel
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Fail to update the MasterFinanceDetails", e));
                return new EventFinanceDetailCommandResult { };
            }
        }
    }
}