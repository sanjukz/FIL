using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.Transaction
{
    public class TransactionReportCommandHandler : BaseCommandHandlerWithResult<TransactionReportCommand, TransactionReportCommandResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public TransactionReportCommandHandler(IReportingRepository reportingRepository, IMediator mediator) : base(mediator)
        {
            _reportingRepository = reportingRepository;
        }

        protected override Task<ICommandResult> Handle(TransactionReportCommand command)
        {
            try
            {
                //IEnumerable<Kz.Contracts.Commands.Transaction.TransactionReport> transactionReport = _reportingRepository.GetAllTransactionReportData(command);
                IEnumerable<FIL.Contracts.Commands.Transaction.TransactionReport> transactionReport = null;

                return Task.FromResult<ICommandResult>(new TransactionReportCommandResult
                {
                    TransactionReport = transactionReport
                });
            }
            catch (System.Exception ex)
            {
                return Task.FromResult<ICommandResult>(new TransactionReportCommandResult());
            }
        }
    }
}