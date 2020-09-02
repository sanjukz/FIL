using FIL.Api.Repositories;
using FIL.Contracts.Commands.FinanceDetails;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class FinancDetailCommandHandler : BaseCommandHandlerWithResult<FinancDetailCommand, FinancDetailCommandResult>
    {
        private readonly IFinanceDetailsRepository _financeDetailRepository;

        public FinancDetailCommandHandler(IFinanceDetailsRepository financeDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _financeDetailRepository = financeDetailRepository;
        }

        protected override Task<ICommandResult> Handle(FinancDetailCommand command)
        {
            var financeData = new FinanceDetail();
            FinancDetailCommandResult saveEventDataResult = new FinancDetailCommandResult();
            financeData = new FinanceDetail
            {
                Id = command.Id,
                AccountNickName = command.AccountNickName,
                AccountNo = command.AccountNo,
                AccountType = command.AccountType,
                BankAccountType = command.BankAccountType,
                IsBankAccountGST = command.IsBankAccountGST,
                PANNo = command.PANNo,
                IsUpdatLater = command.IsUpdatLater,
                FinancialsAccountBankAccountGSTInfo = command.FinancialsAccountBankAccountGSTInfo,
                StateId = command.BankStateId,
                BankName = command.BankName,
                CountryId = command.CountryId,
                CurrencyId = command.CurrencyId,
                GSTNo = command.GSTNo,
                EventId = command.EventId,
                RoutingNo = command.RoutingNo,
                FirstName = command.FirstName,
                LastName = command.LastName,
                CreatedBy = command.ModifiedBy,
                ModifiedBy = command.ModifiedBy,
                CreatedUtc = DateTime.Now
            };
            FIL.Contracts.DataModels.FinanceDetail result = null;
            try
            {
                result = _financeDetailRepository.Save(financeData);
            }
            catch (Exception ex)
            {
                // handle exception
            }
            saveEventDataResult.Id = result.Id;
            return Task.FromResult<ICommandResult>(saveEventDataResult);
        }
    }
}