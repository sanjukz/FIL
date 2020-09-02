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
    public class SaveExOzSessionCommandHandler : BaseCommandHandlerWithResult<SaveExOzSessionCommand, SaveExOzSessionCommandResult>
    {
        private readonly IExOzProductRepository _exOzProductRepository;
        private readonly IExOzProductSessionRepository _exOzProductSessionRepository;

        private SaveExOzSessionCommandResult updatedSessions = new SaveExOzSessionCommandResult()
        {
            SessionList = new List<ExOzProductSession>()
        };

        public SaveExOzSessionCommandHandler(IExOzProductSessionRepository exOzProductSessionRepository, IEventTicketDetailRepository eventTicketDetailRepository, ITicketCategoryRepository ticketCategoryRepository, IExOzProductRepository exOzProductRepository, IMediator mediator)
            : base(mediator)
        {
            _exOzProductSessionRepository = exOzProductSessionRepository;
            _exOzProductRepository = exOzProductRepository;
        }

        protected override Task<ICommandResult> Handle(SaveExOzSessionCommand command)
        {
            _exOzProductSessionRepository.DisableAllExOzProductSessions();

            UpdateAllSessions(command);
            return Task.FromResult<ICommandResult>(updatedSessions);
        }

        protected void UpdateAllSessions(SaveExOzSessionCommand command)
        {
            int i = 0;
            List<long> apiProductSessionIds = command.SessionList.Select(w => w.Id).ToList();
            var exOzProductSessions = _exOzProductSessionRepository.GetBySessionIds(apiProductSessionIds);

            foreach (var item in command.SessionList)
            {
                try
                {
                    ExOzProductSession exOzProductSession = exOzProductSessions.Where(w => w.ProductSessionId == item.Id).FirstOrDefault();
                    ExOzProduct exOzProduct = _exOzProductRepository.GetByProductId(item.ProductId);

                    ExOzProductSession retProductSession = UpdateProductSession(item, exOzProductSession, exOzProduct.Id, command.ModifiedBy);
                    updatedSessions.SessionList.Add(retProductSession);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        protected ExOzProductSession UpdateProductSession(ExOzSessionResponse item, ExOzProductSession exOzProductSession, long productId, Guid ModifiedBy)
        {
            ExOzProductSession exOzProductSessionInserted = new ExOzProductSession();
            if (exOzProductSession == null)
            {
                var newExOzProductSession = new ExOzProductSession
                {
                    SessionName = item.SessionName,
                    ProductSessionId = item.Id,
                    ProductId = productId,
                    HasPickups = item.HasPickups,
                    Levy = Convert.ToDecimal(item.Levy),
                    IsExtra = item.IsExtra,
                    TourHour = item.TourHour,
                    TourMinute = item.TourMinute,
                    TourDuration = item.TourDuration,
                    ModifiedBy = ModifiedBy,
                    IsEnabled = true,
                };
                exOzProductSessionInserted = _exOzProductSessionRepository.Save(newExOzProductSession);
            }
            else
            {
                exOzProductSession.IsEnabled = true;
                exOzProductSession.ModifiedBy = ModifiedBy;
                exOzProductSessionInserted = _exOzProductSessionRepository.Save(exOzProductSession);
            }
            return exOzProductSessionInserted;
        }
    }
}