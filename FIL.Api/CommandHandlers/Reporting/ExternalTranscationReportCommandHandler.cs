using FIL.Api.Repositories;
using FIL.Contracts.Commands.Reporting;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Reporting
{
    public class ExternalTranscationReportCommandHandler : BaseCommandHandler<ExternalReportCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public ExternalTranscationReportCommandHandler(ITransactionRepository transactionRepository, IMediator mediator) : base(mediator)
        {
            _transactionRepository = transactionRepository;
        }

        protected override async Task Handle(ExternalReportCommand command)
        {
            DateTime FromDate = new DateTime(2010, 01, 01);
            DateTime ToDate = DateTime.UtcNow;
            ReportExportStatus CurrentExportStatus = ReportExportStatus.None;
            ReportExportStatus NewExportStatus = ReportExportStatus.None;

            if (command.FromDate != null)
            {
                FromDate = (DateTime)command.FromDate;
            }

            if (command.ToDate != null)
            {
                ToDate = (DateTime)command.ToDate;
            }

            if (command.CurrentExportStatus != null)
            {
                CurrentExportStatus = (ReportExportStatus)command.CurrentExportStatus;
            }

            if (command.NewExportStatus != null)
            {
                NewExportStatus = (ReportExportStatus)command.NewExportStatus;
            }

            var transactionsList = new List<FIL.Contracts.DataModels.Transaction>();

            transactionsList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.Transaction>>(_transactionRepository.GetTransactionsByUserDatesAndStatus(command.UserAltId, FromDate, ToDate, CurrentExportStatus).ToList());

            if (transactionsList.Any())
            {
                if (NewExportStatus != ReportExportStatus.None)
                {
                    foreach (FIL.Contracts.DataModels.Transaction transaction in transactionsList)
                    {
                        transaction.ReportExportStatus = NewExportStatus;
                        FIL.Contracts.DataModels.Transaction transactionResult = _transactionRepository.Save(transaction);
                    }
                }
            }
        }
    }
}